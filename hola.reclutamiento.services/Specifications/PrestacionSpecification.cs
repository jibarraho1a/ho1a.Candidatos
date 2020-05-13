using ho1a.reclutamiento.models.Candidatos;

namespace ho1a.reclutamiento.services.Specifications
{
    public class PrestacionSpecification : BaseSpecification<Prestacion>
    {
        public PrestacionSpecification(int idPrestacion)
            : base(a => a.Id == idPrestacion)
        {
            this.AddInclude(a => a.UltimoTrabajo);
            this.AddInclude(a => a.UltimoTrabajo.CandidatoDetalle);
            this.AddInclude(a => a.UltimoTrabajo.CandidatoDetalle.Candidato);
        }
    }
}
