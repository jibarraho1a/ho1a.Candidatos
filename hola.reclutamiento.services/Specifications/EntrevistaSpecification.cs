using ho1a.reclutamiento.models.Plazas;
using System.Linq;

namespace ho1a.reclutamiento.services.Specifications
{
    public class EntrevistaSpecification : BaseSpecification<Entrevista>
    {
        public EntrevistaSpecification(int idRequisicion)
            : base(
                a =>
                a.Competencias.Any(c => c.PlantillaEntrevista.RequisicionDetalle.RequisicionId == idRequisicion))
        {
            this.AddInclude(a => a.Candidato);
            this.AddInclude(a => a.Competencias);
        }

        public EntrevistaSpecification(string userName)
            : base(a => a.Entrevistador.ToUpper() == userName.ToUpper())
        {
        }

        public EntrevistaSpecification(int idRequisicion, int idEntrevista)
            : base(a => a.Id == idEntrevista)
        {
            this.AddInclude(a => a.Candidato);
            this.AddInclude(a => a.Candidato.CandidatoUser);
            this.AddInclude(a => a.Candidato.CandidatoDetalle.UltimoTrabajo);
            this.AddInclude(a => a.Candidato.CandidatoDetalle.PuestoSolicitado);
            this.AddInclude(a => a.Competencias);
            this.AddInclude("Competencias.PlantillaEntrevista");
        }
    }
}
