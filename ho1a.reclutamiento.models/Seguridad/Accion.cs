using ho1a.applicationCore.Entities;

namespace ho1a.reclutamiento.models.Seguridad
{
    public class Accion : BaseEntityId
    {
        public AccionPermisos AccionPermisos { get; set; }
        public int? AccionPermisosId { get; set; }
        public Componente Componente { get; set; }
        public int ComponenteId { get; set; }
        public string Descripcion { get; }
        public TipoAccion TipoAccion { get; set; }
    }
}
