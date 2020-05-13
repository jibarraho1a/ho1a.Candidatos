using ho1a.applicationCore.Entities;
using System;

namespace ho1a.reclutamiento.models.Plazas
{
    public class Reasignacion : BaseEntityId
    {
        public string Comentario { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaTermino { get; set; }
        public Requisicion Requisicion { get; set; }
        public int? RequisicionId { get; set; }
        public string UserNameFrom { get; set; }
        public string UserNameTo { get; set; }
    }
}