using ho1a.reclutamiento.models.Candidatos;

namespace ho1a.reclutamiento.services.Specifications
{
    public class ExpedienteArchivoSpecification : BaseSpecification<ExpedienteArchivo>
    {
        public ExpedienteArchivoSpecification(int idCandidato)
            : base(a => a.CandidatoExpediente.CandidatoDetalle.CandidatoId == idCandidato)
        {
            this.AddInclude(a => a.File);
            this.AddInclude(a => a.Expediente);
            this.AddInclude(a => a.CandidatoExpediente);

        }
    }
}
