using ho1a.reclutamiento.models.Catalogos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Specifications
{
    public class PuestoSolicitadoSpecification : BaseSpecification<PuestoSolicitado>
    {
        public PuestoSolicitadoSpecification(string adamId)
            : base(a => a.Active && a.AdamId == adamId)
        {
        }
    }
}
