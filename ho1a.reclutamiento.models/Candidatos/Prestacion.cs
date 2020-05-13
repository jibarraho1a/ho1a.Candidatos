using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.enums.Plazas;

namespace ho1a.reclutamiento.models.Candidatos
{
    public class Prestacion : BaseEntityId
    {
        public string Nombre { get; set; }
        public ETipoPrestacion TipoPrestacion { get; set; }
        public UltimoTrabajo UltimoTrabajo { get; set; }
        public int UltimoTrabajoId { get; set; }
        public string Valor { get; set; }
    }
}