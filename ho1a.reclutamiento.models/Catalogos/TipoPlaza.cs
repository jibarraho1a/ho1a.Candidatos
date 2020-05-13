using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.models.Plazas;
using System.Collections.Generic;

namespace ho1a.reclutamiento.models.Catalogos
{
    public class TipoPlaza : BaseEntityId
    {
        public string Descripcion { get; set; }
        public ICollection<Requisicion> Requisiciones { get; set; }
    }
}