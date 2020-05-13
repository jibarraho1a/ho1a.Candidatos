using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.enums.Seguridad;

namespace ho1a.reclutamiento.models.Seguridad
{
    public class Validacion : BaseEntityId
    {
        public ETipoValidacion Nombre { get; set; }
        public int PermisoVistaId { get; set; }
        public ETipoDato TipoDato { get; set; }
        public string Descripcion { get; set; }
        public string Valor { get; set; }
    }
}