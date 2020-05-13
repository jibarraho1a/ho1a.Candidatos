using System.Collections.Generic;

namespace ho1a.reclutamiento.services.ViewModels.Candidato
{
    public class TernaViewModel : BaseViewModel
    {
        public List<CandidatoViewModel> Candidatos { get; set; }
        public string Terna { get; set; }
        public int TernaId { get; set; }
    }
}
