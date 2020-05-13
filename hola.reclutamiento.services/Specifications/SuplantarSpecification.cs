using ho1a.reclutamiento.models.Seguridad;

namespace ho1a.reclutamiento.services.Specifications
{
    public class SuplantarSpecification : BaseSpecification<Suplantar>
    {
        private readonly string userName;

        public SuplantarSpecification(string userName)
            : base(a => a.UserLogin == userName)
        {
            this.userName = userName;
        }
    }
}
