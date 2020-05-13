using ho1a.reclutamiento.models.Seguridad;
using ho1a.reclutamiento.models.Configuracion;
using ho1a.reclutamiento.models.Plazas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ho1a.Api.HttpClient;
using ho1a.reclutamiento.enums.Plazas;
using ho1a.applicationCore.Utilerias;

namespace Reclutamiento.Controllers.Candidato
{
    using ho1a.applicationCore.Data.Interfaces;
    using ho1a.reclutamiento.models.Candidatos;
    using ho1a.reclutamiento.services.Data.Interfaces;
    using ho1a.reclutamiento.services.Services.Interfaces;
    using ho1a.reclutamiento.services.Specifications;
    using ho1a.reclutamiento.services.ViewModels.Candidato;
    using ho1a.reclutamiento.services.ViewModels.Plazas;

    [Authorize]
    [Route("api/[controller]")]
    [EnableCors("miRHPolicy")]
    public class CandidatoController : BaseController<Candidato, CandidatoViewModel, CandidatoViewModel>
    {
        private readonly IAsyncRepository<Candidato> candidatoRepository;
        private readonly ICandidatoService candidatoService;
        private readonly IGlobalConfiguration<Configuracion> configuracion;
        private readonly IMapper mapper;
        private readonly INotificarService notificarService;
        private readonly IAsyncRepository<Ternas> ternasRepository;
        private readonly IRequisicionService requisicionService;
        private readonly UserManager<CandidatoUser> userManager;

        public CandidatoController(
          UserManager<CandidatoUser> userManager,
          IUserResolverService userResolverService,
          IMapper mapper,
          IGeneralService<Candidato> service,
          IAsyncRepository<Candidato> candidatoRepository,
          ICandidatoService candidatoService,
          IGlobalConfiguration<Configuracion> configuracion,
          IRequisicionService requisicionService,
          INotificarService notificarService)
          : base(userResolverService, mapper, service)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.candidatoRepository = candidatoRepository;
            this.candidatoService = candidatoService;
            this.configuracion = configuracion;
            this.notificarService = notificarService;
            this.requisicionService = requisicionService;
        }

        [HttpGet("{idCandidato}")]
        public override async Task<ActionResult<CandidatoViewModel>> Get(string UserName, int idCandidato)
        {
            try
            {
                this.Specification = new CandidatoSpecification(idCandidato);
                var candidato = await base.Get(UserName, idCandidato)
                                        .ConfigureAwait(false);
                return candidato;
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [Route("Catalogo")]
        [HttpGet]
        public async Task<ActionResult<List<CandidatoBusquedaViewModel>>> GetCatalogo()
        {
            try
            {
                var candidato = await this.candidatoService.GetAsync(new CandidatoSpecification())
                                        .ConfigureAwait(false);

                var result = this.mapper.Map<List<CandidatoBusquedaViewModel>>(candidato);
                return result;
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [Route("Requisicion/{idRequisicion}")]
        [HttpGet("{idRequisicion}")]
        public async Task<ActionResult<List<CandidatoViewModel>>> GetRequisicionById(int idRequisicion)
        {
            try
            {
                var ternas = await this.requisicionService.GetTernasByRequisicionAsync(idRequisicion)
                                     .ConfigureAwait(false);

                var candidatos = new List<CandidatoViewModel>();

                foreach (var terna in ternas)
                {
                    foreach (var ternaCandidato in terna.TernaCandidato)
                    {
                        var mapper = this.mapper;
                        var candidato = mapper?.Map<CandidatoViewModel>(ternaCandidato.Candidato);
                        if (candidato == null)
                        {
                            continue;
                        }

                        candidato.Terna = terna.TipoTerna.GetDescription();
                        candidato.TernaId = (int)terna.TipoTerna;
                        candidato.Entrevistas = mapper?.Map<List<EntrevistaViewModel>>(ternaCandidato.Entrevistas);
                        candidatos.Add(candidato);
                    }
                }

                return candidatos;
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpPost("{idRequisicion}")]
        public async Task<ActionResult<CandidatoViewModel>> Post(string UserName, int idRequisicion, [FromBody] CandidatoViewModel entity)
        {
            try
            {
                var candidato = this.mapper?.Map<Candidato>(entity);

                if (candidato == null)
                {
                    return this.NotFound();
                }

                if (candidato.CandidatoUser != null)
                {
                    return this.NotFound();
                }

                await this.AddNewCandidato(candidato, entity)
                  .ConfigureAwait(false);

                await this.requisicionService.AddCandidatoAsync(idRequisicion, new List<Candidato> { candidato }, true)
                  .ConfigureAwait(false);

                return await this.Get(UserName, candidato.Id)
                               .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public override async Task<ActionResult<CandidatoViewModel>> Put(string UserName, int id, [FromBody] CandidatoViewModel entity)
        {
            try
            {
                var candidato = this.mapper.Map<Candidato>(entity);
                var result = await this.candidatoService.UpdateCandidatoAsync(id, candidato)
                                     .ConfigureAwait(false);

                return await this.Get(UserName, id)
                               .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [Route("AddTerna/{idRequisicion}/{idCandidato}/{idTerna}")]
        [HttpPut("{idRequisicion}/{idCandidato}/{idTerna}")]
        public async Task<ActionResult> PutAddTerna(int idRequisicion, int idCandidato, int idTerna)
        {
            try
            {
                await this.candidatoService.AddCandidatoToTernaAsync(idRequisicion, idCandidato, (ETerna)idTerna)
                  .ConfigureAwait(false);

                return this.Ok();
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [Route("VerificarCorreo/{correoCandidato}")]
        [HttpGet]
        public async Task<ActionResult<bool>> VerificarCorreo(string correoCandidato)
        {
            try
            {
                var usuario = await this.userManager.FindByEmailAsync(correoCandidato)
                                      .ConfigureAwait(false);

                return usuario != null;
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [Route("UpdateCV/{idCandidato}")]
        [HttpPut("{idCandidato}")]
        public async Task<ActionResult> ResumenCV(int idCandidato, [FromBody] ResumenCVViewModel resumenCV)
        {
            try
            {
                var candidato = this.mapper.Map<Candidato>(resumenCV);
                var result = await this.candidatoService.AddResumenCVToCandidatoAsync(idCandidato, candidato).ConfigureAwait(false);

                return result ? this.Ok() : (ActionResult)this.NotFound();
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        private async Task AddNewCandidato(Candidato candidato, CandidatoViewModel entity)
        {
            //var resultCandidato = await this.candidatoRepository.AddAsync(candidato)
            //                              .ConfigureAwait(false);

            var resultCandidato = await this.candidatoService.AddCandidatoAsync(candidato);

            var userIdentity = new CandidatoUser
            {
                UserName = entity.Email,
                CandidatoId = resultCandidato.Id,
                Email = entity.Email
            };

            var password = this.configuracion.Configuration<string>("PwdNuevoCandidato");
            var result = await this.userManager.CreateAsync(userIdentity, password)
                                 .ConfigureAwait(false);

            if (result.Succeeded)
            {
                var user = await this.userManager.FindByNameAsync(entity.Email)
                                   .ConfigureAwait(false);

                if (user == null)
                {
                    return;
                }

                var urlApi = this.configuracion.Configuration<string>("UrlCandidatoAplicacion");

                var forgotPasswordModel = new { email = user.Email, Alta = true };

                await HttpRequestFactory.PostAsync($"{urlApi}api/Candidato/auth/ForgotPassword", forgotPasswordModel)
                  .ConfigureAwait(false);
            }
            else
            {
                throw new Exception("Usuario duplicado.");
            }
        }
    }
}
