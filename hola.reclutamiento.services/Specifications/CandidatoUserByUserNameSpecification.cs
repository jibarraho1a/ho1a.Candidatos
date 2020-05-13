using ho1a.reclutamiento.models.Seguridad;

namespace ho1a.reclutamiento.services.Specifications
{
    public class CandidatoUserByUserNameSpecification : BaseSpecification<CandidatoUser>
    {
        public CandidatoUserByUserNameSpecification(string email)
            : base(a => a.Email == email)
        {
        }
    }
}
