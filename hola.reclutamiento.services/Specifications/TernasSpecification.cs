using ho1a.reclutamiento.models.Plazas;
using System.Linq;

namespace ho1a.reclutamiento.services.Specifications
{
    public class TernasSpecification : BaseSpecification<Ternas>
    {
        public TernasSpecification(int idRequisicion)
            : base(a => a.RequisicionDetalle.RequisicionId == idRequisicion)
        {
            this.AddInclude(r => r.TernaCandidato);
            this.AddInclude("TernaCandidato.Candidato");
        }

        public TernasSpecification(int idRequisicion, int idCandidato)
            : base(
                a => a.RequisicionDetalle.RequisicionId == idRequisicion
                     && a.TernaCandidato.Any(t => t.CandidatoId == idCandidato))
        {
            this.AddInclude(r => r.TernaCandidato);
            this.AddInclude("TernaCandidato.Candidato");
        }
    }
}
