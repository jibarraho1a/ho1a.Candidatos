using ho1a.reclutamiento.models.Configuracion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services.Interfaces
{
    public interface IConfiguracionGlobalService
    {
        Task<Configuracion> AddConfiguracionesGlobalesAsync(Configuracion configuracion);

        Task<bool> DeleteConfiguracionGlobalAsync(int idConfiguracionGlobal);

        Task<List<Configuracion>> GetConfiguracionGlobalAsync();

        Task<Configuracion> GetConfiguracionGlobalAsync(int idConfiguracionGlobal);

        Task<Configuracion> UpdateConfiguracionGlobalAsync(int idConfiguracionGlobal, Configuracion configuracionGlobal);
    }
}