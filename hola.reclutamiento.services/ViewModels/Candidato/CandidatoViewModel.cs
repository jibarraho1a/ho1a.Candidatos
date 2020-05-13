using ho1a.reclutamiento.enums.Candidatos;
using ho1a.reclutamiento.services.ViewModels.Plazas;
using System.Collections.Generic;

namespace ho1a.reclutamiento.services.ViewModels.Candidato
{
    public class CandidatoViewModel : BaseViewModel
    {
        public int? CandidatoId { get; set; }
        public CandidatoDetalleViewModel Detalle { get; set; }
        public int? DetalleId { get; set; }
        public string Email { get; set; }
        public List<EntrevistaViewModel> Entrevistas { get; set; }
        public string Materno { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public EEstadoCandidato StatusCandidato { get; set; }
        public EStatusCapturaCandidato StatusCaptura { get; set; }
        public string Terna { get; set; }
        public int TernaId { get; set; }
        public string UserAd { get; set; }
    }
}
