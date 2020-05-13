using ho1a.reclutamiento.models.Candidatos;

namespace ho1a.reclutamiento.services.Specifications
{
    public sealed class CandidatoExpedienteSpecification : BaseSpecification<CandidatoExpediente>
    {
        public CandidatoExpedienteSpecification(int idCandidato)
            : base(a => a.CandidatoDetalle.CandidatoId == idCandidato)
        {
            this.AddInclude(a => a.CandidatoDetalle);
            this.AddInclude(a => a.CandidatoDetalle.Candidato);
            this.AddInclude(a => a.CandidatoDetalle.Candidato.CandidatoUser);
            this.AddInclude(a => a.ExpedientesArchivos);
            this.AddInclude("ExpedientesArchivos.Expediente");
            this.AddInclude("ExpedientesArchivos.CandidatoExpediente");
            this.AddInclude("ExpedientesArchivos.CandidatoExpediente.CandidatoDetalle");
            this.AddInclude(
                "ExpedientesArchivos.CandidatoExpediente.ExpedientesArchivos.CandidatoExpediente.CandidatoDetalle");
            this.AddInclude("ExpedientesArchivos.CandidatoExpediente.ExpedientesArchivos");
            this.AddInclude("ExpedientesArchivos.CandidatoExpediente.ExpedientesArchivos.File");
        }
    }
}
