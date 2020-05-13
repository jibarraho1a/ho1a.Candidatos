using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.enums.Notificacion;

namespace ho1a.reclutamiento.models.Notificacion
{
    public class NotificacionCorreos : BaseEntityId
    {
        public string Asunto { get; set; }
        public string Descripcion { get; set; }
        public ETipoEvento Evento { get; set; }
        public string Nombre { get; set; }
        public string Notificacion { get; set; }
        public int? VistaId { get; set; }
    }
}