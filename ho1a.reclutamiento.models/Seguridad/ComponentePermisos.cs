using ho1a.applicationCore.Entities;

namespace ho1a.reclutamiento.models.Seguridad
{
    public class ComponentePermisos : BaseEntityId
    {
        public Componente Componente { get; set; }
        public int ComponenteId { get; set; }
        public bool Edicion { get; set; }
        public RolUser Rol { get; set; }
        public bool Visible { get; set; }
    }
}