using AutoMapper;
using ho1a.reclutamiento.models.Catalogos;
using ho1a.reclutamiento.services.Services.Interfaces;
using ho1a.reclutamiento.services.ViewModels.Plazas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Reclutamiento.Controllers.Catalogos
{
    [Route("api/[controller]")]
    [EnableCors("miRHPolicy")]
    [Authorize]
    public class DocumentoExpedienteController : BaseController<Expediente, ExpedienteViewModel, ExpedienteViewModel>
    {
        public DocumentoExpedienteController(
            IUserResolverService userResolverService,
            IMapper mapper,
            IGeneralService<Expediente> service)
            : base(userResolverService, mapper, service)
        {
        }
    }
}