using ho1a.reclutamiento.models.Seguridad;

namespace ho1a.reclutamiento.services.Specifications
{
    public sealed class VisibilidadSpecification : BaseSpecification<Componente>
    {
        public VisibilidadSpecification()
            : base(a => a.Active)
        {
            this.AddInclude(a => a.Componentes);
            this.AddInclude(a => a.Validaciones);
            this.AddInclude(a => a.Acciones);
            this.AddInclude(a => a.Permiso);
            this.AddInclude("Acciones");
            this.AddInclude("Acciones.AccionPermisos");
            this.AddInclude("Acciones.TipoAccion");
            this.AddInclude("Componentes.Acciones");
            this.AddInclude("Componentes.Acciones.AccionPermisos");
            this.AddInclude("Componentes.Acciones.TipoAccion");
            this.AddInclude("Componentes.Validaciones");
            this.AddInclude("Componentes.Vista");
            this.AddInclude("Componentes.Permiso");
        }

        public VisibilidadSpecification(int idPantalla)
            : base(a => a.VistaId == idPantalla && a.ComponentePadre == null)
        {
            this.AddInclude(a => a.Componentes);
            this.AddInclude(a => a.Validaciones);
            this.AddInclude(a => a.Acciones);
            this.AddInclude(a => a.Permiso);
            this.AddInclude("Acciones");
            this.AddInclude("Acciones.AccionPermisos");
            this.AddInclude("Acciones.AccionPermisos.Accion");
            this.AddInclude("Acciones.TipoAccion");
            this.AddInclude("Componentes.Acciones");
            this.AddInclude("Componentes.Acciones.AccionPermisos");
            this.AddInclude("Componentes.Acciones.TipoAccion");
            this.AddInclude("Componentes.Validaciones");
            this.AddInclude("Componentes.Vista");
            this.AddInclude("Componentes.Permiso");
        }

        public VisibilidadSpecification(int idPantalla, RolUser rolId)
            : base(a => a.VistaId == idPantalla && a.ComponentePadre == null && a.Permiso.Rol == rolId)
        {
            this.AddInclude(a => a.Componentes);
            this.AddInclude(a => a.Validaciones);
            this.AddInclude(a => a.Acciones);
            this.AddInclude(a => a.Permiso);
            this.AddInclude("Acciones");
            this.AddInclude("Acciones.AccionPermisos");
            this.AddInclude("Acciones.AccionPermisos.Accion");
            this.AddInclude("Acciones.TipoAccion");

            this.AddInclude("Componentes.Acciones");
            this.AddInclude("Componentes.Acciones.AccionPermisos");
            this.AddInclude("Componentes.Acciones.AccionPermisos.Accion");
            this.AddInclude("Componentes.Acciones.TipoAccion");
            this.AddInclude("Componentes.Validaciones");
            this.AddInclude("Componentes.Vista");
            this.AddInclude("Componentes.Permiso");
            this.AddInclude("Componentes.Permiso.Rol");

            this.AddInclude("Componentes.Componentes.Acciones");
            this.AddInclude("Componentes.Componentes.Acciones.AccionPermisos");
            this.AddInclude("Componentes.Componentes.Acciones.AccionPermisos.Accion");
            this.AddInclude("Componentes.Componentes.Acciones.TipoAccion");
            this.AddInclude("Componentes.Componentes.Validaciones");
            this.AddInclude("Componentes.Componentes.Vista");
            this.AddInclude("Componentes.Componentes.Permiso");
            this.AddInclude("Componentes.Componentes.Permiso.Rol");
        }
    }
}
