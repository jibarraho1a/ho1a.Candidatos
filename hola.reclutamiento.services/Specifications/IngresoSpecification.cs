using ho1a.reclutamiento.models.Candidatos;

namespace ho1a.reclutamiento.services.Specifications
{
    public sealed class IngresoSpecification : BaseSpecification<Ingreso>
    {
        public IngresoSpecification(int idIngreso)
            : base(a => a.Id == idIngreso)
        {
            this.AddInclude(a => a.UltimoTrabajo);
            this.AddInclude(a => a.UltimoTrabajo.CandidatoDetalle);
            this.AddInclude(a => a.UltimoTrabajo.CandidatoDetalle.Candidato);
        }
    }
}
