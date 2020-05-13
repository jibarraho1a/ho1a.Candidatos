using ho1a.reclutamiento.models.Candidatos;

namespace ho1a.reclutamiento.services.Specifications
{
    public class CandidatoUserSpecification : BaseSpecification<Candidato>
    {
        public CandidatoUserSpecification(string userName)
            : base(a => a.CandidatoUser.UserAd == userName)
        {
            this.AddInclude(a => a.CandidatoUser);
            this.AddInclude(a => a.CandidatoUser.Candidato.CandidatoDetalle);
        }
    }
}
