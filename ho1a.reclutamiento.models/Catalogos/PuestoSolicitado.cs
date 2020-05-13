using ho1a.applicationCore.Entities;

namespace ho1a.reclutamiento.models.Catalogos
{
    public class PuestoSolicitado : BaseEntityId
    {
        public string AdamId { get; set; }
        public string Descripcion { get; set; }
        public string Nombre { get; set; }
        public string Responsabilidad { get; set; }
    }
}
