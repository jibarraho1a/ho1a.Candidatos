using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ho1a.reclutamiento.models.Configuracion;
using ho1a.reclutamiento.models.Plazas;
using ho1a.reclutamiento.services.Services.Interfaces;
using ho1a.applicationCore.Data.Interfaces;
using ho1a.reclutamiento.services.Specifications;
using ho1a.reclutamiento.services.ViewModels.Requisicion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Reclutamiento.Controllers.Plazas
{
    using ho1a.reclutamiento.models.Candidatos;

    [Authorize]
    [Route("api/[controller]")]
    [EnableCors("miRHPolicy")]
    public class ComplementoExpedienteController : ControllerBase
    {
        private readonly ICandidatoService candidatoService;
        private readonly IAsyncRepository<Configuracion> configuracionRepository;
        private readonly IMapper mapper;
        private readonly IRequisicionService requisicionService;
        private readonly IUserResolverService userResolverService;
        private readonly IUserService userService;

        public ComplementoExpedienteController(
            IUserService userService,
            IUserResolverService userResolverService,
            IAsyncRepository<Configuracion> configuracionRepository,
            IRequisicionService requisicionService,
            ICandidatoService candidatoService,
            IMapper mapper)
        {
            this.userService = userService;
            this.userResolverService = userResolverService;
            this.configuracionRepository = configuracionRepository;
            this.requisicionService = requisicionService;
            this.candidatoService = candidatoService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<RequisicionPreviewViewModel>>> Get(bool isAdministradorExpediente, bool isAdministrador)
        {
            try
            {
                if (isAdministradorExpediente || isAdministrador)
                {
                    var result = await this.candidatoService.GetAsync(new CandidatoVistaExpedienteSpecification());

                    var candidatos = this.mapper.Map<List<RequisicionPreviewViewModel>>(result);

                    return this.Ok(candidatos);
                }

                return this.Unauthorized();
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [HttpGet("{idCandidato}")]
        public async Task<ActionResult<RequisicionPreviewViewModel>> Get(int idCandidato, bool isAdministradorExpediente, bool isAdministrador)
        {
            try
            {
                if (!isAdministradorExpediente
                    && !isAdministrador)
                {
                    return this.Unauthorized();
                }

                var result = this.candidatoService.Single(new CandidatoVistaExpedienteSpecification(idCandidato));

                var candidatos = this.mapper.Map<RequisicionPreviewViewModel>(result);
                candidatos.Tareas = await this.GetUsuarioActividades();

                return this.Ok(candidatos);
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [HttpPut("{idCandidato}")]
        public async Task<ActionResult<bool>> Put(
            int idCandidato, bool isAdministradorExpediente, bool isAdministrador, 
            [FromBody] RequisicionPreviewViewModel toUpdate)
        {
            try
            {
                if (!isAdministradorExpediente
                    && !isAdministrador)
                {
                    return this.Unauthorized();
                }

                var candidato = this.mapper.Map<Candidato>(toUpdate);
                await this.candidatoService.ComplementoExpedienteAsync(idCandidato, candidato);

                return this.Ok(true);
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        /// <summary>
        /// The get usuario actividades.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task<List<TareasAlta>> GetUsuarioActividades()
        {
            var configuraciones = await this.configuracionRepository.ListAllAsync()
                                      .ConfigureAwait(false);

            var itemToFind = new List<string>
                                 {
                                     "UserBussinesPartnerRH",
                                     "UserEspecialistaContabilidad",
                                     "UserAltaExpense",
                                     "UserAltaBeneficiosFlexibles",
                                     "UserAsesorRS",
                                     "UserAsignacionLugarEstacionamiento",
                                     "UserEntregaTarjetaAccesoOficina",
                                     "UserEntregaTarjetaAccesoEdificio",
                                     "UserEntregaTarjetaBancaria",
                                     "UserTI"
                                 };

            var tareas = new List<TareasAlta>();

            foreach (var keyConfiguracion in itemToFind)
            {
                var configuracion = configuraciones.FirstOrDefault(c => c.Key == keyConfiguracion);

                if (configuracion == null)
                {
                    continue;
                }

                var tarea = new TareasAlta();

                if (configuracion.Values != null)
                {
                    var usuarios = configuracion.Values.Split(';')
                    .ToList();

                    foreach (var usuario in usuarios)
                    {
                        var user = await this.userService.GetUserByUserNameAsync(usuario);

                        if (user != null)
                        {
                            tarea.Usuarios.Add(user);
                        }
                    }
                }

                if (configuracion.Values2 != null)
                {
                    var actividades = configuracion.Values2.Split(';').ToList();

                    foreach (var actividad in actividades)
                    {
                        tarea.Actividades.Add(actividad);
                    }
                }

                tareas.Add(tarea);
            }

            return tareas;
        }
    }
}