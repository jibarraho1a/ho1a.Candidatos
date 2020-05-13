using ho1a.applicationCore.Entities;

namespace ho1a.reclutamiento.models.ODS
{
    public class DatosAgrTrab : BaseEntity
    {
        public string Agrupacion { get; set; }
        public string Dato { get; set; }
        public string Descripcion { get; set; }
    }
}