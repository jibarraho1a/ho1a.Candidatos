using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ho1a.reclutamiento.models.Seguridad
{
    public class RolUser : IdentityRole
    {
        public RolUser()
        {
        }

        public RolUser(string roleName)
            : base(roleName)
        {
        }

        public ICollection<ComponentePermisos> ComponentesPermisos { get; set; }
    }
}
