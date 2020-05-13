using ho1a.applicationCore.Entities;
using System;
using System.Collections.Generic;

namespace ho1a.reclutamiento.models.Plazas
{
    public class RequisicionPropuesta : BaseEntityId
    {
        public string Beneficios { get; set; }
        public decimal? Bonos { get; set; }
        public string Comentarios { get; set; }
        public DateTime? FechaContestacion { get; set; }
        public DateTime? FechaEnvioPropuesta { get; set; }
        public bool? PropuestaAceptada { get; set; }
        public ICollection<RequisicionArchivo> PropuestaArchivos { get; set; }
        public RequisicionDetalle RequisicionDetalle { get; set; }
        public int RequisicionDetalleId { get; set; }
        public decimal? Salario { get; set; }
    }
}