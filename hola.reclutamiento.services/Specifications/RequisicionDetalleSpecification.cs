using ho1a.reclutamiento.models.Plazas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Specifications
{
    public class RequisicionDetalleSpecification : BaseSpecification<RequisicionDetalle>
    {
        /// <inheritdoc />
        public RequisicionDetalleSpecification()
            : base(a => a.Active)
        {
        }

        public RequisicionDetalleSpecification(int idRequisicion)
            : base(a => a.RequisicionId == idRequisicion)
        {
            this.AddInclude(a => a.Propuestas);
            this.AddInclude(a => a.Requisicion);
            this.AddInclude(a => a.Requisicion.PuestoSolicitado);
            this.AddInclude("Propuestas.PropuestaArchivos");
            this.AddInclude("Propuestas.PropuestaArchivos.File");
            this.AddInclude("Propuestas.PropuestaArchivos.Expediente");
            this.AddInclude(a => a.Ternas);
            this.AddInclude("Ternas.TernaCandidato");
            this.AddInclude("Ternas.TernaCandidato.Candidato.CandidatoDetalle");
            this.AddInclude("Ternas.TernaCandidato.Candidato.CandidatoDetalle.UltimoSalario");
            this.AddInclude("Ternas.TernaCandidato.Candidato.CandidatoDetalle.UltimoTrabajo");
            this.AddInclude("Ternas.TernaCandidato.Entrevistas");
            this.AddInclude("Ternas.TernaCandidato.Entrevistas.Competencias");
            this.AddInclude("Ternas.TernaCandidato.Entrevistas.Competencias.PlantillaEntrevista");
            this.AddInclude("Ternas.TernaCandidato.Ternas");
        }
    }
}
