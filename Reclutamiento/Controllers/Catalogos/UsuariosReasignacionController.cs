using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ho1a.reclutamiento.models.Plazas;
using ho1a.reclutamiento.services.Services.Interfaces;
using ho1a.reclutamiento.services.ViewModels.Candidato;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Reclutamiento.Controllers.Catalogos
{
    [Route("api/[controller]")]
    [EnableCors("miRHPolicy")]
    [Authorize]
    public class UsuariosReasignacionController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IGeneralService<UsuarioReasignacion> service;
        private readonly IUserResolverService userResolverService;
        private readonly IUserService userService;

        public UsuariosReasignacionController(
            IUserResolverService userResolverService,
            IMapper mapper,
            IGeneralService<UsuarioReasignacion> service,
            IUserService userService)
        {
            this.userResolverService = userResolverService;
            this.mapper = mapper;
            this.service = service;
            this.userService = userService;
        }

        public async Task<ActionResult<List<SelectListItem>>> GetAsync()
        {
            try
            {
                var items = await this.service.GetAsync()
                                      .ConfigureAwait(false);
                var viewModels = this.mapper.Map<List<UsuarioReasignacionViewModel>>(items);
                foreach (var item in viewModels)
                {
                    var user = await this.userService.GetUserByUserNameAsync(item.UserName)
                                         .ConfigureAwait(false);

                    item.Nombre = user?.ToString();
                }

                return this.mapper?.Map<List<SelectListItem>>(viewModels);
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }
    }
}