using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.models.Candidatos;
using ho1a.reclutamiento.models.Catalogos;

namespace ho1a.reclutamiento.models.Plazas
{
    public class RequisicionArchivo : BaseEntityId
    {
        public Expediente Expediente { get; set; }
        public int ExpedienteId { get; set; }
        public FileUpload File { get; set; }
        public RequisicionPropuesta RequisicionPropuesta { get; set; }
        public int RequisicionPropuestaId { get; set; }
    }
}