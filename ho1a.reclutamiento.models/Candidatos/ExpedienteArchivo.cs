using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.models.Catalogos;

namespace ho1a.reclutamiento.models.Candidatos
{
    public class ExpedienteArchivo : BaseEntityId
    {
        public CandidatoExpediente CandidatoExpediente { get; set; }

        public int? CandidatoExpedienteId { get; set; }

        public Expediente Expediente { get; set; }

        public int? ExpedienteId { get; set; }

        public FileUpload File { get; set; }
    }
}