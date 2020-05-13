using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ho1a.applicationCore.Data.Interfaces;
using ho1a.reclutamiento.models.Plazas;
using ho1a.reclutamiento.services.Data.Interfaces;
using ho1a.reclutamiento.services.Services.Interfaces;
using ho1a.reclutamiento.services.Specifications;
using ho1a.reclutamiento.services.ViewModels.Plazas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Reclutamiento.Controllers.Plazas
{
    [Route("api/[controller]")]
    [EnableCors("miRHPolicy")]
    [Authorize]
    public class ComentarioController : BaseController<Entrevista, EntrevistaViewModel, EntrevistaViewModel>
    {
        private readonly ICandidatoService candidatoService;
        private readonly IAsyncRepository<Entrevista> entrevistaRepository;
        private readonly IMapper mapper;

        public ComentarioController(
            IUserResolverService userResolverService,
            IMapper mapper,
            IGeneralService<Entrevista> service,
            IAsyncRepository<Entrevista> entrevistaRepository,
            ICandidatoService candidatoService)
            : base(userResolverService, mapper, service)
        {
            this.mapper = mapper;
            this.entrevistaRepository = entrevistaRepository;
            this.candidatoService = candidatoService;
        }

        [HttpPut("{idRequisicion}/{idEntrevista}")]
        public async Task<ActionResult> Put(
            string UserName,
            int idRequisicion,
            int idEntrevista,
            [FromBody] ComentarioViewModel comentario)
        {
            try
            {
                var resultEEntrevistas = await this.entrevistaRepository.ListAsync(
                    new EntrevistaSpecification(idRequisicion, idEntrevista))
                                                   .ConfigureAwait(false);

                var entrevista = resultEEntrevistas.FirstOrDefault();

                entrevista.Comentarios = comentario.Comentario;

                var entrevistaToEdit = this.mapper.Map<EntrevistaViewModel>(entrevista);

                await this.Put(UserName, idEntrevista, entrevistaToEdit)
                    .ConfigureAwait(false);

                return this.Ok();
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }
    }
}