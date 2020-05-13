using ho1a.reclutamiento.models.Candidatos;

namespace ho1a.reclutamiento.services.Specifications
{
    public class ReferenciaLaboralSpecification : BaseSpecification<ReferenciaLaboral>
    {
        public ReferenciaLaboralSpecification(int idReferenciaLaboral)
            : base(a => a.Id == idReferenciaLaboral)
        {
            this.AddInclude(a => a.CandidatoDetalle);
            this.AddInclude(a => a.CandidatoDetalle.Candidato);
        }
    }
}
