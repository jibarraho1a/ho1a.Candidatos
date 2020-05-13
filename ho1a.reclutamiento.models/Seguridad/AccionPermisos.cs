using ho1a.applicationCore.Entities;

namespace ho1a.reclutamiento.models.Seguridad
{
    public class AccionPermisos : BaseEntityId
    {
        public Accion Accion { get; set; }
        public int AccionId { get; set; }
        public bool Activo { get; set; }
        public RolUser Rol { get; set; }
        public bool Visible { get; set; }
    }
}