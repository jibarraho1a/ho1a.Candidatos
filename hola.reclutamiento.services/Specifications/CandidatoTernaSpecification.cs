using ho1a.reclutamiento.models.Plazas;

namespace ho1a.reclutamiento.services.Specifications
{
    public class CandidatoTernaSpecification : BaseSpecification<TernaCandidato>
    {
        public CandidatoTernaSpecification(int idRequisicion)
            : base(a => a.Ternas.RequisicionDetalle.RequisicionId == idRequisicion)
        {
            this.AddInclude(r => r.Ternas);
            this.AddInclude(r => r.Ternas.RequisicionDetalle);
            this.AddInclude(r => r.Candidato);
            this.AddInclude(r => r.Candidato.CandidatoDetalle);
            this.AddInclude(r => r.Entrevistas);          
        }
    }
}
