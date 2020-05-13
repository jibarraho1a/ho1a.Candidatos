using System;

namespace ho1a.Reclutamiento.DAL
{
    public abstract class  ModelBase
    {
        public int Id { get; set; }
        public string CreadoPor { get; set; }
        public string EditadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaEditado { get; set; }
        public bool Activo { get; set; }
    }
}
