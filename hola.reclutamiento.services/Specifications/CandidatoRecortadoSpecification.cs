using ho1a.reclutamiento.models.Candidatos;

namespace ho1a.reclutamiento.services.Specifications
{
    public sealed class CandidatoRecortadoSpecification : BaseSpecification<Candidato>
    {
        public CandidatoRecortadoSpecification(int idCandidato)
            : base(a => a.Id == idCandidato)
        {
            this.AddInclude(a => a.CandidatoDetalle);
        }
    }
}
