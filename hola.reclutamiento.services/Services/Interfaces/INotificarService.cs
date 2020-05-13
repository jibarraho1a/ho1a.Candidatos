using System.Threading.Tasks;
using ho1a.reclutamiento.services.ViewModels.Notificaciones;

namespace ho1a.reclutamiento.services.Services.Interfaces
{
    public interface INotificarService
    {
        Task<bool> NotificarAsync(NotificacionViewModel notificacionViewModel);
    }
}
