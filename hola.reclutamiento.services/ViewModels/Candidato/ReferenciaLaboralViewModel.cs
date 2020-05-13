namespace ho1a.reclutamiento.services.ViewModels.Candidato
{
    public class ReferenciaLaboralViewModel : BaseViewModel
    {
        public int CandidatoDetalleId { get; set; }
        public string Cargo { get; set; }
        public string Email { get; set; }
        public string Materno { get; set; }
        public string Nombre { get; set; }
        public string Parentesco { get; set; }
        public string Paterno { get; set; }
        public bool SolicitarReferencia { get; set; }
        public string TiempoConocerse { get; set; }
    }
}
