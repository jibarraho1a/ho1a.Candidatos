using ho1a.reclutamiento.models.Candidatos;
using System;

namespace ho1a.reclutamiento.services.Specifications
{
    public sealed class CandidatoSpecification : BaseSpecification<Candidato>
    {
        /// <inheritdoc />
        public CandidatoSpecification()
            : base(a => a.CandidatoUser != null && !string.IsNullOrEmpty(a.CandidatoUser.Email))
        {
            this.AddInclude(a => a.CandidatoDetalle);
            this.AddInclude(a => a.CandidatoDetalle.PuestoSolicitado);
            this.AddInclude(a => a.CandidatoUser);
        }

        public CandidatoSpecification(int idCandidato)
            : base(a => a.Id == idCandidato)
        {
            this.AddInclude(a => a.CandidatoDetalle);
            this.AddInclude(a => a.CandidatoUser);
            this.AddInclude(a => a.CandidatoUser.Candidato);
            this.AddInclude(a => a.CandidatoDetalle.CandidatoExpediente.CandidatoDetalle);
            this.AddInclude(a => a.CandidatoDetalle.CandidatoExpediente);
            this.AddInclude(a => a.CandidatoDetalle.CandidatoExpediente.ExpedientesArchivos);
            this.AddInclude("CandidatoDetalle.CandidatoExpediente.ExpedientesArchivos.Expediente");
            this.AddInclude("CandidatoDetalle.CandidatoExpediente.ExpedientesArchivos.File");
            this.AddInclude(a => a.CandidatoDetalle.Cv);
            this.AddInclude(a => a.CandidatoDetalle.Direccion);
            this.AddInclude(a => a.CandidatoDetalle.EstadoCivil);
            this.AddInclude(a => a.CandidatoDetalle.ReferenciaVacante);
            this.AddInclude(a => a.CandidatoDetalle.ReferenciasLaborales);
            this.AddInclude(a => a.CandidatoDetalle.ReferenciasPersonales);
            this.AddInclude(a => a.CandidatoDetalle.UltimoTrabajo);
            this.AddInclude(a => a.CandidatoDetalle.UltimoTrabajo.Prestaciones);
            this.AddInclude(a => a.CandidatoDetalle.UltimoTrabajo.Ingresos);
        }

        public CandidatoSpecification(Guid idCandidato)
            : base(a => a.CandidatoUser.Id == idCandidato.ToString())
        {
            this.AddInclude(a => a.CandidatoDetalle);
            this.AddInclude(a => a.CandidatoUser);
            this.AddInclude(a => a.CandidatoDetalle.CandidatoExpediente.ExpedientesArchivos);
            this.AddInclude("CandidatoDetalle.CandidatoExpediente.ExpedientesArchivos.Expediente");
            this.AddInclude("CandidatoDetalle.CandidatoExpediente.ExpedientesArchivos.File");
            this.AddInclude(a => a.CandidatoDetalle.Cv);
            this.AddInclude(a => a.CandidatoDetalle.Direccion);
            this.AddInclude(a => a.CandidatoDetalle.EstadoCivil);
            this.AddInclude(a => a.CandidatoDetalle.ReferenciaVacante);
            this.AddInclude(a => a.CandidatoDetalle.ReferenciasLaborales);
            this.AddInclude(a => a.CandidatoDetalle.ReferenciasPersonales);
            this.AddInclude(a => a.CandidatoDetalle.UltimoTrabajo);
            this.AddInclude(a => a.CandidatoDetalle.UltimoTrabajo.Prestaciones);
            this.AddInclude(a => a.CandidatoDetalle.UltimoTrabajo.Ingresos);
        }

        public CandidatoSpecification(string email)
            : base(a => a.CandidatoUser.Email == email)
        {
            this.AddInclude(a => a.CandidatoUser);
        }
    }
}
