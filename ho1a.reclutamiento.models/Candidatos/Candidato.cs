using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.models.Seguridad;

namespace ho1a.reclutamiento.models.Candidatos
{
    public class Candidato : BaseEntityId
    {
        public Candidato()
        {
            this.CandidatoDetalle = new CandidatoDetalle();
        }

        public CandidatoDetalle CandidatoDetalle { get; set; }

        public CandidatoUser CandidatoUser { get; set; }

        public string Materno { get; set; }

        public string Nombre { get; set; }

        public string Paterno { get; set; }

        public override string ToString()
        {
            return $"{this.Nombre ?? string.Empty} {this.Paterno ?? string.Empty} {this.Materno ?? string.Empty}";
        }
    }
}