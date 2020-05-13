using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.enums.Candidatos;

namespace ho1a.reclutamiento.models.Plazas
{
    public class EntrevistaResumen : BaseEntityId
    {
        public EEstadoCandidato EstadoCandidato { get; set; }
        public RequisicionDetalle RequisicionCandidato { get; set; }
        public int? RequisicionCandidatoId { get; set; }
    }
}