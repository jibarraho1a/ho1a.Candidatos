using System;
using System.Threading.Tasks;
using ho1a.reclutamiento.models.Seguridad;
using ho1a.reclutamiento.services.Services.Interfaces;
using ho1a.Reclutamiento.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Reclutamiento.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private IUserService userService;
        private ITokenService tokenService;
        private ho1a.Reclutamiento.BLL.Services.Interfaces.IRequisicionService requisicionService;

        public LoginController(
            IUserService userService,
            ITokenService tokenService,
            ho1a.Reclutamiento.BLL.Services.Interfaces.IRequisicionService requiService)
        {
            this.userService = userService;
            this.tokenService = tokenService;
            this.requisicionService = requiService;
        }

        [HttpGet]
        [Route("test")]
        public ActionResult Test()
        {
            requisicionService.Actualizar(new Requisicion());
            return Ok();
        }

        [HttpPost]
        [Route("authenticate")]
        public async Task<ActionResult<dynamic>> ValidateUserAsync(string username)
        {
            try
            {
                User user = new User();
                user = await userService.GetUserByUserNameAsync("jtiradol", false).ConfigureAwait(false);

                var token = new JsonWebToken
                {
                    Acces_Token = tokenService.CreateToken(user, DateTime.UtcNow.AddHours(8)),
                    Expires_in = 480//minutes
                };

                user.Token = token;

                return user;
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}