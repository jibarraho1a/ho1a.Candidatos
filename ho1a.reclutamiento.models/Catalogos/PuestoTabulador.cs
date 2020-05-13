using ho1a.applicationCore.Entities;

namespace ho1a.reclutamiento.models.Catalogos
{
    public class PuestoTabulador : BaseEntityId
    {
        public PuestoSolicitado Puesto { get; set; }

        public TabuladorSalario Tabulador { get; set; }
    }
}