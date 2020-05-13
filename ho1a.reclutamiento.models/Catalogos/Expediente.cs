using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.enums.Catalogos;
using ho1a.reclutamiento.models.Candidatos;
using ho1a.reclutamiento.models.Plazas;
using System.Collections.Generic;

namespace ho1a.reclutamiento.models.Catalogos
{
    public class Expediente : BaseEntityId
    {
        public string Descripcion { get; set; }
        public ICollection<ExpedienteArchivo> ExpedienteArchivos { get; set; }
        public string Nombre { get; set; }
        public bool Requerido { get; set; }
        public ICollection<RequisicionArchivo> RequisicionArchivos { get; set; }
        public ETipoArchivo TipoArchivo { get; set; }
        public ETipoComponente TipoComponente { get; set; }
    }
}
