using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.enums.Plazas;
using System.Collections.Generic;

namespace ho1a.reclutamiento.models.Plazas
{
    public class Ternas : BaseEntityId
    {
        public RequisicionDetalle RequisicionDetalle { get; set; }
        public int? RequisicionDetalleId { get; set; }
        public ICollection<TernaCandidato> TernaCandidato { get; set; }
        public ETerna TipoTerna { get; set; }
    }
}