using ho1a.reclutamiento.models.Catalogos;
using ho1a.reclutamiento.models.ODS;
using ho1a.reclutamiento.models.Plazas;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services.Interfaces
{
    public interface IInformacionTrabajoService
    {
        Task<List<SelectListItem>> GetAllCatalogoPuestos(int idColaborador);

        Task<List<SelectListItem>> GetAllCatalogoPuestos(int idColaborador, string idAlias);

        Task<List<SelectListItem>> GetCatalogoPuestos(int idColaborador, string idAlias);

        Task<List<SelectListItem>> GetCatalogoPuestos(int idColaborador);

        Task<InformacionTrabajo> GetInformacionTrabajoAsync(int idColaborador, int idPuesto, string idAlias);

        Task<TabuladorSalario> GetTabuladorAsync(int idColaborador, int idPuesto);

        Task<List<Plazas>> GetVacantesByIdTrabajadorAsync(int idTrabajador);

        Task<List<Plazas>> GetVacantesByIdTrabajadorAsync(int idTrabajador, string idAlias);

        Task<List<SelectListItem>> GetAliasByTrabajadorPuestoAsync(int idTrabajador, string idPuesto);
    }
}
