using System;
using System.Threading.Tasks;
using ho1a.reclutamiento.enums.Notificacion;
using ho1a.reclutamiento.services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Reclutamiento.Controllers.Candidato
{
    [Route("api/[controller]")]
    [EnableCors("miRHPolicy")]
    [Authorize]
    public class NotificarController : ControllerBase
    {
        private readonly ICandidatoService candidatoService;

        public NotificarController(ICandidatoService candidatoService)
        {
            this.candidatoService = candidatoService;
        }

        [Route("Candidato/{idCandidato}")]
        public async Task<ActionResult<bool>> Post(int idCandidato)
        {
            try
            {
                return await this.candidatoService.NotificarAsync(idCandidato, ETipoEvento.AltaCandidato)
                                 .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }
    }
}