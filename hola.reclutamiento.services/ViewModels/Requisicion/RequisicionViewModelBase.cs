using ho1a.reclutamiento.models.Plazas;
using System.Collections.Generic;

namespace ho1a.reclutamiento.services.ViewModels.Requisicion
{
    public class RequisicionViewModelBase
    {
        public string UserRequisicion { get; set; }
        public List<ValidacionRequisicionPlaza> Validaciones { get; set; }
    }
}
