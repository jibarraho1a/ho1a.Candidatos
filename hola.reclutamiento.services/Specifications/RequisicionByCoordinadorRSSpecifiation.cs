using ho1a.reclutamiento.enums.Candidatos;
using ho1a.reclutamiento.enums.Plazas;
using ho1a.reclutamiento.models.Plazas;
using System.Linq;

namespace ho1a.reclutamiento.services.Specifications
{
    public sealed class RequisicionByCoordinadorRSSpecifiation : BaseSpecification<Requisicion>
    {
        public RequisicionByCoordinadorRSSpecifiation(string userName, bool isCoordinadorRS)
            : base(
                a =>
                isCoordinadorRS && a.ValidaRequisiciones.All(v => v.EstadoValidacion == EEstadoValidacion.Aprobada)
                && a.RequisicionDetalle.Ternas.Any(
                    t => t.TernaCandidato.Any(tc => tc.EstadoCandidato == EEstadoCandidato.Idoneo)))
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
            this.AddInclude("RequisicionDetalle.Ternas.TernaCandidato.Candidato.CandidatoDetalle.CandidatoExpediente");
            this.AddInclude(r => r.PuestoSolicitado);
            this.AddInclude(r => r.TabuladorSalario);
            this.AddInclude(r => r.RequisicionDetalle.Propuestas);
        }
    }
}
