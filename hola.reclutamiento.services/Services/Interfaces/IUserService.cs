using ho1a.reclutamiento.models.Seguridad;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services.Interfaces
{
    public interface IUserService
    {
        bool ValidateUser(Credenciales cred);

        Task<User> GetUserByIdColaboradorAsync(int idColaborador);

        Task<User> GetUserByUserNameAsync(string userName, bool? detalle = null);

        Task<List<User>> GetUserNamesByUserNameAsync(string userName);

        bool IsAdmin(string userName);
    }
}
