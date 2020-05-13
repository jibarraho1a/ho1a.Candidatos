using ho1a.reclutamiento.enums.Candidatos;
using ho1a.reclutamiento.models.Plazas;
using System.Linq;

namespace ho1a.reclutamiento.services.Specifications
{
    public sealed class CandidatoIdoneoSpecification : BaseSpecification<RequisicionDetalle>
    {
        /// <inheritdoc />
        public CandidatoIdoneoSpecification(int idRequisicion)
            : base(
                a => a.RequisicionId == idRequisicion && a.Ternas.Any(
                         t => t.TernaCandidato.Any(tc => tc.EstadoCandidato == EEstadoCandidato.Idoneo)))
        {
            this.AddInclude(a => a.Ternas);
            this.AddInclude("Ternas.TernaCandidato.Candidato");
            this.AddInclude("Ternas.TernaCandidato.Candidato.CandidatoUser");
            this.AddInclude("Ternas.TernaCandidato.Candidato.CandidatoDetalle");
            this.AddInclude("Ternas.TernaCandidato.Candidato.CandidatoDetalle.CandidatoExpediente");
            this.AddInclude("Ternas.TernaCandidato.Candidato.CandidatoDetalle.CandidatoExpediente.ExpedientesArchivos");
            this.AddInclude(
                "Ternas.TernaCandidato.Candidato.CandidatoDetalle.CandidatoExpediente.ExpedientesArchivos.File");
            this.AddInclude("Ternas.TernaCandidato.Candidato.CandidatoDetalle.ReferenciasLaborales");
            this.AddInclude("Ternas.TernaCandidato.Candidato.CandidatoDetalle.ReferenciasPersonales");
            this.AddInclude("Ternas.TernaCandidato.Candidato.CandidatoDetalle.ReferenciaVacante");
            this.AddInclude("Ternas.TernaCandidato.Candidato.CandidatoDetalle.UltimoSalario");
            this.AddInclude("Ternas.TernaCandidato.Candidato.CandidatoDetalle.UltimoTrabajo");
            this.AddInclude("Ternas.TernaCandidato.Candidato.CandidatoDetalle.UltimoTrabajo.Ingresos");
            this.AddInclude("Ternas.TernaCandidato.Candidato.CandidatoDetalle.UltimoTrabajo.Prestaciones");
            this.AddInclude("Ternas.TernaCandidato.Candidato.CandidatoDetalle.EstadoCivil");
            this.AddInclude("Ternas.TernaCandidato.Candidato.CandidatoDetalle.Direccion");
            this.AddInclude("Ternas.TernaCandidato.Candidato.CandidatoDetalle.Cv");
        }
    }
}
