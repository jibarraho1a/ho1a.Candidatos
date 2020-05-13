using ho1a.reclutamiento.models.Seguridad;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services.Interfaces
{
    public interface IUserResolverService
    {
        bool ValidateUser(Credenciales cred);

        Task<User> GetUserODSAsync(string userNameOrigin);

        Task<bool> HasEntrevistas(string userName);

        bool IsAdminExpediente(string userName);

        Task<bool> IsAdministrador(string userName);

        Task<bool> IsAdministradorExpediente(string userName);

        bool IsAdministradorRh(string userName);

        Task<bool> IsAutorizador(string userName);

        bool IsCoordinadorRs(string userName);

        Task<bool> IsReclutador(string userName);
    }
}
