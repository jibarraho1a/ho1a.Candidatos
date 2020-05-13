using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.enums.Plazas;

namespace ho1a.reclutamiento.models.Candidatos
{
    public class Ingreso : BaseEntityId
    {
        public decimal Monto { get; set; }
        public string Nombre { get; set; }
        public ETipoSalario TipoIngreso { get; set; }
        public UltimoTrabajo UltimoTrabajo { get; set; }
        public int UltimoTrabajoId { get; set; }
    }
}