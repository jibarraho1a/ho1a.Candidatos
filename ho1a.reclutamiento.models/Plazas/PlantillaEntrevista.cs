using ho1a.applicationCore.Entities;

namespace ho1a.reclutamiento.models.Plazas
{
    public class PlantillaEntrevista : BaseEntityId
    {
        public string Descripcion { get; set; }
        public string Nombre { get; set; }
        public RequisicionDetalle RequisicionDetalle { get; set; }
        public int? RequisicionDetalleId { get; set; }
    }
}