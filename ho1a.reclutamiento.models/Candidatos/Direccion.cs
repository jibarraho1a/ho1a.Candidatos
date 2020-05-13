using ho1a.applicationCore.Entities;

namespace ho1a.reclutamiento.models.Candidatos
{
    public class Direccion : BaseEntityId
    {
        public string Calle { get; set; }
        public CandidatoDetalle CandidatoDetalle { get; set; }
        public int? CandidatoDetalleId { get; set; }
        public string CodigoPostal { get; set; }
        public string Colonia { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
    }
}