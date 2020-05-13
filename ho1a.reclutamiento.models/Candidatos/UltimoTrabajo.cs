using ho1a.applicationCore.Entities;
using System.Collections.Generic;

namespace ho1a.reclutamiento.models.Candidatos
{
    public class UltimoTrabajo : BaseEntityId
    {
        public UltimoTrabajo()
        {
            this.Prestaciones = new List<Prestacion>();
            this.Ingresos = new List<Ingreso>();
        }
        public CandidatoDetalle CandidatoDetalle { get; set; }
        public int? CandidatoDetalleId { get; set; }
        public string Empresa { get; set; }
        public bool GastosMedicosMayores { get; set; }
        public ICollection<Ingreso> Ingresos { get; set; }
        public ICollection<Prestacion> Prestaciones { get; set; }
        public string Puesto { get; set; }
        public bool SeguroVida { get; set; }
        public decimal? SueldoFijoMensual { get; set; }
    }
}