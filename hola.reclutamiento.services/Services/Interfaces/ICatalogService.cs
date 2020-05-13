using ho1a.reclutamiento.models.Catalogos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<List<PuestoSolicitado>> GetListPuestosAsync();
        Task<IEnumerable<SelectListItem>> GetSelectListEmpresaAsync();
        Task<IEnumerable<SelectListItem>> GetSelectListEstadosCivilAsync();
        Task<IEnumerable<SelectListItem>> GetSelectListLocalidadesAsync();
        Task<IEnumerable<SelectListItem>> GetSelectListMotivosIngresoAsync();
        Task<IEnumerable<SelectListItem>> GetSelectListPuestos();
        Task<IEnumerable<SelectListItem>> GetSelectListReferenciaVancanteAsync();
        Task<IEnumerable<SelectListItem>> GetSelectListTiposPlazaAsync();
        Task<IEnumerable<SelectListItem>> GetSelectListUltimoSalarioAsync();
    }
}
