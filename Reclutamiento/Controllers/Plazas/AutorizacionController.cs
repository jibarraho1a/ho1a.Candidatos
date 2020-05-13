using System;
using System.Linq;
using System.Threading.Tasks;
using ho1a.applicationCore.Utilerias;
using ho1a.reclutamiento.enums.Plazas;
using ho1a.reclutamiento.services.Services.Interfaces;
using ho1a.reclutamiento.services.Specifications;
using ho1a.reclutamiento.services.ViewModels.Requisicion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Reclutamiento.Controllers.Plazas
{
    [Route("api/[controller]")]
    [EnableCors("miRHPolicy")]
    [Authorize]
    public class AutorizacionController : ControllerBase
    {
        private readonly IAutorizacionService autorizacionService;
        private readonly IRequisicionService requisicionService;
        private readonly IUserResolverService userResolverService;
        private readonly IUserService userService;

        public AutorizacionController(
            IUserService userService,
            IUserResolverService userResolverService,
            IRequisicionService requisicionService,
            IAutorizacionService autorizacionService)
        {
            this.userService = userService;
            this.userResolverService = userResolverService;
            this.requisicionService = requisicionService;
            this.autorizacionService = autorizacionService;
        }

        [HttpPut("{idRequisicion}")]
        public async Task<ActionResult> Put(
            int idRequisicion,
            [FromBody] ValidacionesRequisicionViewModel validacion = null)
        {
            try
            {
                var requisicion = this.requisicionService.Single(new RequisicionSpecification(idRequisicion));

                if (validacion == null)
                {
                    if (requisicion.ValidaRequisiciones.Any())
                    {

                        var toValidate =
                            requisicion.ValidaRequisiciones.FirstOrDefault(
                                v => v.NivelValidacion == ENivelValidacion.Requeridor);

                        validacion = new ValidacionesRequisicionViewModel
                        {
                            Active = toValidate.Active,
                            Date = DateTime.Now,
                            Description = toValidate.Comentario,
                            Id = toValidate.Id,
                            Info = toValidate.UserValidador?.ToString(),
                            Name = toValidate.NivelValidacion.GetDescription(),
                            NivelValidacion = toValidate.NivelValidacion,
                            StateValidation = EEstadoValidacion.Aprobada,
                            UserName = toValidate.AprobadorUserName
                        };

                        await this.autorizacionService.AprobacionAsync(
                            idRequisicion,
                            requisicion.UserRequeridor,
                            requisicion.MotivoIngreso.Descripcion,
                            validacion)
                            .ConfigureAwait(true);
                    }
                    else
                    {
                        await this.autorizacionService.SolicitarAutorizacionAsync(
                            idRequisicion,
                            requisicion.UserRequeridor,
                            requisicion.MotivoIngreso.Descripcion)
                            .ConfigureAwait(true);
                    }
                }
                else
                {
                    await this.autorizacionService.AprobacionAsync(
                        idRequisicion,
                        requisicion.UserRequeridor,
                        requisicion.MotivoIngreso.Descripcion,
                        validacion)
                        .ConfigureAwait(true);
                }

                await this.requisicionService.SetAsignacionAsync(requisicion, null, false)
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