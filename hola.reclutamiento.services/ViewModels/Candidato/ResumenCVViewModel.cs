using Microsoft.AspNetCore.Mvc.Rendering;

namespace ho1a.reclutamiento.services.ViewModels.Candidato
{
    public class ResumenCVViewModel : BaseViewModel
    {
        public string Empresa { get; set; }
        public string Puesto { get; set; }
        public bool Certificacion { get; set; }
        public decimal? PretencionEconomica { get; set; }
        public SelectListItem UltimoSalario { get; set; }
        public int Experiencia { get; set; }
    }
}
