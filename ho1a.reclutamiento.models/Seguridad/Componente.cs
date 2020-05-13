using ho1a.applicationCore.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ho1a.reclutamiento.models.Seguridad
{
    public class Componente : BaseEntityId
    {
        public ICollection<Accion> Acciones { get; set; }
        public Componente ComponentePadre { get; set; }
        [Column(Order = 1)]
        public int? ComponentePadreId { get; set; }
        public ICollection<Componente> Componentes { get; set; }
        public string Descripcion { get; set; }
        public int Indice { get; set; }
        public string Nombre { get; set; }
        public ComponentePermisos Permiso { get; set; }
        public ICollection<Validacion> Validaciones { get; set; }
        public Vista Vista { get; set; }
        public int VistaId { get; set; }
    }
}