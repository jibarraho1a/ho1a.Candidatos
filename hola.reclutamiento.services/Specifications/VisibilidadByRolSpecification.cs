using ho1a.reclutamiento.models.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Specifications
{
    public class VisibilidadByRolSpecification : BaseSpecification<RolUser>
    {
        public VisibilidadByRolSpecification(int idPantalla, RolUser rolUser)
          : base(a => a.ComponentesPermisos.Any(c => c.Rol == rolUser))
        {
            this.AddInclude(a => a.ComponentesPermisos);
            this.AddInclude("Componentes.Validaciones");
            this.AddInclude("Componentes.Acciones");
            this.AddInclude("Componentes.Permiso");
        }
    }
}
