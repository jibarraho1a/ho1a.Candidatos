using ho1a.reclutamiento.enums.Notificacion;
using ho1a.reclutamiento.models.Notificacion;
using System.Collections.Generic;

namespace ho1a.reclutamiento.services.ViewModels.Notificaciones
{
    public class NotificacionViewModel
    {
        public IList<Attachment> Attachments { get; set; }
        public string Body { get; set; }
        public IList<string> Cc { get; set; }
        public IList<string> Cco { get; set; }
        public string FromDisplay { get; set; }
        public string FromMail { get; set; }
        public dynamic Item { get; set; }
        public string Subject { get; set; }
        public ETipoEvento TipoEvento { get; set; }
        public IList<string> ToMail { get; set; }
    }
}
