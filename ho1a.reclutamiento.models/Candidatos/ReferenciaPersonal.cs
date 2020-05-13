namespace ho1a.reclutamiento.models.Candidatos
{
    public class ReferenciaPersonal : Persona
    {
        public CandidatoDetalle CandidatoDetalle { get; set; }
        public int? CandidatoDetalleId { get; set; }
        public string Parentesco { get; set; }
        public bool SolicitarReferencia { get; set; }
        public string Telefono { get; set; }
        public string TiempoConocerse { get; set; }
    }
}