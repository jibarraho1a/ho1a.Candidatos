using ho1a.reclutamiento.models.Seguridad;

namespace ho1a.reclutamiento.services.Specifications
{
    public class VisibilidadRecortadoSpecification : BaseSpecification<Componente>
    {
        public VisibilidadRecortadoSpecification(int idPantalla, RolUser rolId, int? componentePadre = null)
          : base(
            a =>
            a.VistaId == idPantalla && a.ComponentePadreId == componentePadre && a.Permiso.Rol == rolId)
        {
            this.AddInclude(a => a.Componentes);
            this.AddInclude(a => a.Validaciones);
            this.AddInclude(a => a.Acciones);
            this.AddInclude(a => a.Permiso);
            this.AddInclude(a => a.Acciones);
            this.AddInclude("Acciones.AccionPermisos");
            this.AddInclude("Acciones.AccionPermisos.Accion");
            this.AddInclude("Acciones.TipoAccion");
        }

        public VisibilidadRecortadoSpecification(int idPantalla, int? componentePadre = null)
          : base(a => a.VistaId == idPantalla && a.ComponentePadreId == componentePadre)
        {
            this.AddInclude(a => a.Componentes);
            this.AddInclude(a => a.Validaciones);
            this.AddInclude(a => a.Acciones);
            this.AddInclude(a => a.Permiso);
            this.AddInclude(a => a.Acciones);
            this.AddInclude("Acciones.AccionPermisos");
            this.AddInclude("Acciones.AccionPermisos.Accion");
            this.AddInclude("Acciones.TipoAccion");
        }
    }
}
