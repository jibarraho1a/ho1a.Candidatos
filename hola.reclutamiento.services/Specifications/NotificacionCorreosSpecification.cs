using ho1a.reclutamiento.enums.Notificacion;
using ho1a.reclutamiento.models.Notificacion;

namespace ho1a.reclutamiento.services.Specifications
{
    public class NotificacionCorreosSpecification : BaseSpecification<NotificacionCorreos>
    {
        public NotificacionCorreosSpecification(int Vista)
            : base(a => a.VistaId == Vista)
        {
        }

        public NotificacionCorreosSpecification(ETipoEvento tipoEvento)
            : base(a => a.Evento == tipoEvento)
        {
        }
    }
}
