using ho1a.reclutamiento.services.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ho1a.reclutamiento.services.ViewModels.Catalogos
{
    public class CatalogoViewModel : BaseViewModel
    {
        public IEnumerable<SelectListItem> EstadosCivil { get; set; }

        public IEnumerable<SelectListItem> LocalidadesSucursa { get; set; }

        public IEnumerable<SelectListItem> PuestoSolicitado { get; set; }

        public IEnumerable<SelectListItem> ReferenciasVacante { get; set; }

        public IEnumerable<SelectListItem> UltimosSalarios { get; set; }
    }
}
