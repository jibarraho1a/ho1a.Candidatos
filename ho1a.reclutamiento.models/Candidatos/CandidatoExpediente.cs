using ho1a.applicationCore.Entities;
using System;
using System.Collections.Generic;

namespace ho1a.reclutamiento.models.Candidatos
{
    public class CandidatoExpediente : BaseEntityId
    {
        public CandidatoDetalle CandidatoDetalle { get; set; }
        public int? CandidatoDetalleId { get; set; }
        public string Comentario { get; set; }
        public bool? CorregirExpediente { get; set; }
        public ICollection<ExpedienteArchivo> ExpedientesArchivos { get; set; }
        public DateTime? FechaNotificacion { get; set; }
        public DateTime? FechaValidaExpediente { get; set; }
    }
}