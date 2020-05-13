using ho1a.reclutamiento.models.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Specifications
{
    public class ValidacionSpecification : BaseSpecification<Validacion>
    {
        public ValidacionSpecification()
            : base(a => a.Active)
        {
        }
    }
}
