using ho1a.applicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ho1a.reclutamiento.models.ODS
{
    public class RelTrabAgr : BaseEntity
    {
        public string Agrupacion { get; set; }
        public string Compania { get; set; }
        public string Dato { get; set; }
        public string Descripcion { get; set; }
        public string Trabajador { get; set; }
    }
}
