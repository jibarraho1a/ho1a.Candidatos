using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.enums.Plazas;
using ho1a.reclutamiento.models.Catalogos;
using ho1a.reclutamiento.models.Seguridad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ho1a.reclutamiento.models.Plazas
{
    public class Requisicion : BaseEntityId
    {
        public Requisicion()
        {
            this.Active = true;
            this.Created = DateTime.Now;
            this.RequisicionDetalle = new RequisicionDetalle();
        }
        public string Alias { get; set; }
        public string AliasId { get; set; }
        public string Departamento { get; set; }
        public string DescripcionTrabajo { get; set; }
        public Empresa Empresa { get; set; }
        public int? EmpresaId { get; set; }
        public DateTime? FechaFin { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public DateTime? InicioReclutamiento { get; set; }
        public Localidad Localidad { get; set; }
        public int? LocalidadId { get; set; }
        public string Mercado { get; set; }
        public MotivoIngreso MotivoIngreso { get; set; }
        public int? MotivoIngresoId { get; set; }
        public string NivelOrganizacional { get; set; }
        public int NumeroVacantes { get; set; }
        public string Observaciones { get; set; }
        public string OtraLocalidad { get; set; }
        public PuestoSolicitado PuestoSolicitado { get; set; }
        public int? PuestoSolicitadoId { get; set; }
        public ICollection<Reasignacion> Reasignaciones { get; set; }
        public RequisicionDetalle RequisicionDetalle { get; set; }
        public string Responsabilidades { get; set; }
        public TabuladorSalario TabuladorSalario { get; set; }
        public int? TabuladorSalarioId { get; set; }
        public ETipoBusqueda TipoBusqueda { get; set; }
        public TipoPlaza TipoPlaza { get; set; }
        public int? TipoPlazaId { get; set; }
        public string UserAsignado { get; set; }
        public string UserRequeridor { get; set; }
        public int VacantesSolicitadas { get; set; }
        public ICollection<ValidaRequisicion> ValidaRequisiciones { get; set; }
        [NotMapped]
        public User User { get; set; }
    }
}