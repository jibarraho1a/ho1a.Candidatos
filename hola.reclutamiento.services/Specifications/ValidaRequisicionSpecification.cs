using ho1a.reclutamiento.models.Plazas;

namespace ho1a.reclutamiento.services.Specifications
{
    public class ValidaRequisicionSpecification : BaseSpecification<ValidaRequisicion>
    {
        public ValidaRequisicionSpecification(int idRequisicion)
            : base(a => a.RequisicionId == idRequisicion)
        {
        }
    }
}
