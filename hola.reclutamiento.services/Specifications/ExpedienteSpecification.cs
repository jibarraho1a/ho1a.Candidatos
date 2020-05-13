using ho1a.reclutamiento.enums.Catalogos;
using ho1a.reclutamiento.models.Catalogos;
using System.Linq;

namespace ho1a.reclutamiento.services.Specifications
{
    public sealed class ExpedienteSpecification : BaseSpecification<Expediente>
    {
        public ExpedienteSpecification(ETipoArchivo tipoArchivo)
            : base(a => a.TipoArchivo == tipoArchivo)
        {
        }

        public ExpedienteSpecification(int idCandidato, ETipoArchivo tipoArchivo)
            : base(
                a => a.TipoArchivo == tipoArchivo && a.ExpedienteArchivos.Any(
                         e => e.CandidatoExpediente.CandidatoDetalle.CandidatoId == idCandidato))
        {
            this.AddInclude(a => a.ExpedienteArchivos);
            this.AddInclude("ExpedienteArchivos.Expediente");
            this.AddInclude("ExpedienteArchivos.Expediente.ExpedienteArchivos");
            this.AddInclude("ExpedienteArchivos.File");
        }

        public ExpedienteSpecification(int idRequisicion, int idRequisicionPropuesta, ETipoArchivo tipoArchivo)
            : base(
                a => a.TipoArchivo == tipoArchivo && a.RequisicionArchivos.Any(
                         e => e.RequisicionPropuestaId == idRequisicionPropuesta
                              && !e.RequisicionPropuesta.FechaContestacion.HasValue
                              && e.RequisicionPropuesta.RequisicionDetalle.RequisicionId == idRequisicion
                              && e.RequisicionPropuesta.Id == idRequisicion))
        {
            this.AddInclude(a => a.RequisicionArchivos);
            this.AddInclude("RequisicionArchivos.Expediente");
            this.AddInclude("RequisicionArchivos.RequisicionPropuesta.RequisicionDetalle");
            this.AddInclude("RequisicionArchivos.RequisicionPropuesta.PropuestaArchivos");
            this.AddInclude("RequisicionArchivos.RequisicionPropuesta.PropuestaArchivos.File");
        }

        public ExpedienteSpecification(int idCandidato)
            : base(
                a => a.TipoArchivo == ETipoArchivo.Expediente && a.ExpedienteArchivos.Any(
                         e => e.CandidatoExpediente.CandidatoDetalle.CandidatoId == idCandidato))
        {
            this.AddInclude(a => a.ExpedienteArchivos);
            this.AddInclude("ExpedienteArchivos.CandidatoExpediente");
            this.AddInclude("ExpedienteArchivos.CandidatoExpediente.CandidatoDetalle");
            this.AddInclude("ExpedienteArchivos.CandidatoExpediente.CandidatoDetalle.Candidato");
            this.AddInclude("ExpedienteArchivos.Expediente");
            this.AddInclude("ExpedienteArchivos.Expediente.ExpedienteArchivos");
            this.AddInclude("ExpedienteArchivos.File");
        }
    }
}
