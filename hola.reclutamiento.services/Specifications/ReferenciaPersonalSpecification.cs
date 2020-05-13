using ho1a.reclutamiento.models.Candidatos;

namespace ho1a.reclutamiento.services.Specifications
{
    public class ReferenciaPersonalSpecification : BaseSpecification<ReferenciaPersonal>
    {
        public ReferenciaPersonalSpecification(int idReferenciaPersonal)
            : base(a => a.Id == idReferenciaPersonal)
        {
            this.AddInclude(a => a.CandidatoDetalle);
            this.AddInclude(a => a.CandidatoDetalle.Candidato);
        }
    }
}
