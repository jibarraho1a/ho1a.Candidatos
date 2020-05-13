using ho1a.reclutamiento.models.Candidatos;

namespace ho1a.reclutamiento.services.Specifications
{
    public sealed class CandidatoVistaExpedienteSpecification : BaseSpecification<Candidato>
    {
        public CandidatoVistaExpedienteSpecification()
            : base(
                a =>
                    (a.CandidatoDetalle.ProcesoAltaCompleta == null || !a.CandidatoDetalle.ProcesoAltaCompleta.Value)
                    && a.CandidatoDetalle.Requisicion.RequisicionDetalle.FechaIngreso
                    != null)
        {
            this.AddInclude(a => a.CandidatoDetalle.Requisicion);
            this.AddInclude(a => a.CandidatoDetalle.Requisicion.Empresa);
            this.AddInclude(a => a.CandidatoDetalle.Requisicion.Localidad);
            this.AddInclude(a => a.CandidatoDetalle.Requisicion.RequisicionDetalle);
            this.AddInclude(a => a.CandidatoDetalle.Requisicion.PuestoSolicitado);
            this.AddInclude(a => a.CandidatoUser);
            this.AddInclude(a => a.CandidatoDetalle);
            this.AddInclude(a => a.CandidatoDetalle.Direccion);
            this.AddInclude(a => a.CandidatoDetalle.EstadoCivil);
        }

        public CandidatoVistaExpedienteSpecification(int idCandidato)
            : base(
                a => a.Id == idCandidato
                     && a.CandidatoDetalle.Requisicion.RequisicionDetalle.FechaIngreso
                     != null)
        {
            this.AddInclude(a => a.CandidatoDetalle.Requisicion);
            this.AddInclude(a => a.CandidatoDetalle.Requisicion.Empresa);
            this.AddInclude(a => a.CandidatoDetalle.Requisicion.Localidad);
            this.AddInclude(a => a.CandidatoDetalle.Requisicion.RequisicionDetalle);
            this.AddInclude(a => a.CandidatoDetalle.Requisicion.PuestoSolicitado);
            this.AddInclude(a => a.CandidatoUser);
            this.AddInclude(a => a.CandidatoDetalle);
            this.AddInclude(a => a.CandidatoDetalle.Direccion);
            this.AddInclude(a => a.CandidatoDetalle.EstadoCivil);
        }
    }
}
