using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.models.Catalogos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ho1a.reclutamiento.models.Plazas
{
    public class RequisicionDetalle : BaseEntityId
    {
        public RequisicionDetalle()
        {
            this.Active = true;
            this.Created = DateTime.Now;
        }
        public bool? AlertaSeguimeinto { get; set; }
        public DateTime? FechaAlertaSeguimiento { get; set; }
        public DateTime? FechaCancelacion { get; set; }
        public DateTime? FechaConfirmacionAlta { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public DateTime? FechaNotificacionAlta { get; set; }
        public ICollection<PlantillaEntrevista> PlantillasEntrevistas { get; set; }

        [NotMapped]
        public RequisicionPropuesta Propuesta => this.Propuestas?.OrderByDescending(p => p.Created)
            .ThenByDescending(p => p.FechaEnvioPropuesta)
            .ThenBy(p => p.FechaContestacion)
            .FirstOrDefault(p => p.Active);
        public ICollection<RequisicionPropuesta> Propuestas { get; set; }
        public Requisicion Requisicion { get; set; }
        public int RequisicionId { get; set; }
        public Salario Tabulador { get; set; }
        public ICollection<Ternas> Ternas { get; set; }
    }
}