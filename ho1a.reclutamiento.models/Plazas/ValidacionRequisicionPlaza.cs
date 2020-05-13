using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.enums.Plazas;

namespace ho1a.reclutamiento.models.Plazas
{
    public class ValidacionRequisicionPlaza : BaseValidaRequisicion
    {
        public ENivelValidacion NivelValidacion { get; set; }

        public BaseUser UserValidador { get; set; }
    }
}