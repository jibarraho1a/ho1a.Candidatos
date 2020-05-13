using ho1a.reclutamiento.models.Plazas;

namespace ho1a.reclutamiento.services.Specifications
{
    public class PlantillaEntrevistaSpecification : BaseSpecification<PlantillaEntrevista>
    {
        public PlantillaEntrevistaSpecification(int idRequisicion)
            : base(a => a.RequisicionDetalleId == null || a.RequisicionDetalle.RequisicionId == idRequisicion)
        {
            this.AddInclude(a => a.RequisicionDetalle);
        }
    }
}
