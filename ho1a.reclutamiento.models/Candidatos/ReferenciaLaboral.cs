using System;
using System.Collections.Generic;
using System.Text;

namespace ho1a.reclutamiento.models.Candidatos
{
    public class ReferenciaLaboral : Persona
    {
        public CandidatoDetalle CandidatoDetalle { get; set; }
        public int? CandidatoDetalleId { get; set; }
        public string Cargo { get; set; }
        public bool SolicitarReferencia { get; set; }
        public string Telefono { get; set; }
        public string TiempoConocerse { get; set; }
    }
}