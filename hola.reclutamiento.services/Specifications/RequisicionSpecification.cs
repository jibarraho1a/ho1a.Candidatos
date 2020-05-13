using ho1a.reclutamiento.enums.Plazas;
using ho1a.reclutamiento.models.Plazas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Specifications
{
    public class RequisicionSpecification : BaseSpecification<Requisicion>
    {
        public RequisicionSpecification(string userName, bool isAdministradores, bool isDireccionRH)
            : base(
                a =>
                    isAdministradores
                    && a.ValidaRequisiciones.Any(
                        v => v.NivelValidacion == ENivelValidacion.Requeridor &&
                             v.EstadoValidacion != EEstadoValidacion.Pendiente))
        {
            this.AddInclude(r => r.TipoPlaza);
            this.AddInclude(r => r.MotivoIngreso);
            this.AddInclude(r => r.Localidad);
            this.AddInclude(r => r.Empresa);
            this.AddInclude(r => r.ValidaRequisiciones);
            this.AddInclude(r => r.Reasignaciones);
            this.AddInclude(r => r.RequisicionDetalle);
            this.AddInclude("RequisicionDetalle.Ternas.TernaCandidato.Entrevistas");
            this.AddInclude("RequisicionDetalle.Ternas.TernaCandidato.Candidato");
            this.AddInclude(r => r.PuestoSolicitado);
            this.AddInclude(r => r.TabuladorSalario);
            this.AddInclude(r => r.RequisicionDetalle.Propuestas);
        }

        public RequisicionSpecification(int id)
            : base(a => a.Id == id)
        {
            this.AddInclude(r => r.RequisicionDetalle.PlantillasEntrevistas);
            this.AddInclude(r => r.RequisicionDetalle.Propuestas);
            this.AddInclude("RequisicionDetalle.Propuestas.PropuestaArchivos");
            this.AddInclude("RequisicionDetalle.Ternas");
            this.AddInclude("RequisicionDetalle.Ternas.TernaCandidato");
            this.AddInclude("RequisicionDetalle.Ternas.TernaCandidato.Entrevistas");
            this.AddInclude("RequisicionDetalle.Ternas.TernaCandidato.Entrevistas.Candidato");
            this.AddInclude("RequisicionDetalle.Ternas.TernaCandidato.Entrevistas.Competencias");
            this.AddInclude("RequisicionDetalle.Propuestas.PropuestaArchivos.Expediente");
            this.AddInclude("RequisicionDetalle.Propuestas.PropuestaArchivos.File");
            this.AddInclude(r => r.Empresa);
            this.AddInclude(r => r.Localidad);
            this.AddInclude(r => r.MotivoIngreso);
            this.AddInclude(r => r.PuestoSolicitado);
            this.AddInclude(r => r.Reasignaciones);
            this.AddInclude(r => r.TipoPlaza);
            this.AddInclude(r => r.ValidaRequisiciones);
            this.AddInclude(r => r.TabuladorSalario);
            this.AddInclude(r => r.RequisicionDetalle.Propuestas);
        }
    }
}
