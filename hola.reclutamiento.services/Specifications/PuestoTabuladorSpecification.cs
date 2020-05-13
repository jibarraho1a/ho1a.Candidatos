using ho1a.reclutamiento.models.Catalogos;

namespace ho1a.reclutamiento.services.Specifications
{
    public class PuestoTabuladorSpecification : BaseSpecification<PuestoTabulador>
    {
        public PuestoTabuladorSpecification(int idPuesto)
            : base(a => a.Puesto.Id == idPuesto)
        {
            this.AddInclude(a => a.Puesto);
            this.AddInclude(a => a.Tabulador);
        }
    }
}
