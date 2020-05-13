using ho1a.reclutamiento.models.Catalogos;

namespace ho1a.reclutamiento.services.Specifications
{
    public class TabuladorSalarioSpecification : BaseSpecification<TabuladorSalario>
    {
        public TabuladorSalarioSpecification(string tabulador)
            : base(a => a.Tabulador == tabulador)
        {
        }
    }
}
