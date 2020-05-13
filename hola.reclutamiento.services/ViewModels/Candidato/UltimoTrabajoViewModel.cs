using System.Collections.Generic;

namespace ho1a.reclutamiento.services.ViewModels.Candidato
{
    public class UltimoTrabajoViewModel : BaseViewModel
    {
        public int? CandidatoId { get; set; }
        public string Empresa { get; set; }
        public bool GastosMedicosMayores { get; set; }
        public List<IngresoViewModel> Ingresos { get; set; }
        public List<PrestacionViewModel> Prestaciones { get; set; }
        public string Puesto { get; set; }
        public bool SeguroVida { get; set; }
        public decimal? SueldoFijoMensual { get; set; }
    }
}
