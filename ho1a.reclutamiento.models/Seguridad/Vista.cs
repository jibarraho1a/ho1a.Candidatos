using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.models.Notificacion;
using System.Collections.Generic;

namespace ho1a.reclutamiento.models.Seguridad
{
    public class Vista : BaseEntityId
    {
        public ICollection<Componente> Componentes { get; set; }
        public string Descripcion { get; set; }
        public string Nombre { get; set; }
        public ICollection<NotificacionCorreos> NotificacionesCorreos { get; set; }
    }
}