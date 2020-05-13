using ho1a.reclutamiento.models.Seguridad;

namespace ho1a.reclutamiento.services.Specifications
{
    public sealed class AccionSpecification : BaseSpecification<Accion>
    {
        public AccionSpecification()
            : base(a => a.Active)
        {
            this.AddInclude(a => a.Componente);
            this.AddInclude(a => a.AccionPermisos);
            this.AddInclude(a => a.AccionPermisos.Rol);
            this.AddInclude(a => a.TipoAccion);
        }
    }
}
