using ho1a.reclutamiento.enums.Plazas;
using System;

namespace ho1a.applicationCore.Entities
{
    public class BaseValidaRequisicion : BaseEntityId
    {
        public string Comentario { get; set; }
        public EEstadoValidacion EstadoValidacion { get; set; }
        public DateTime? Fecha { get; set; }
        public string AprobadorUserName { get; set; }
    }
}