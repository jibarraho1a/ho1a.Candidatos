using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.enums.Candidatos;
using ho1a.reclutamiento.models.Candidatos;
using System.Collections.Generic;

namespace ho1a.reclutamiento.models.Plazas
{
    public class TernaCandidato : BaseEntityId
    {
        public TernaCandidato()
        {
            this.Active = true;
        }
        public Candidato Candidato { get; set; }
        public int? CandidatoId { get; set; }
        public ICollection<Entrevista> Entrevistas { get; set; }
        public EEstadoCandidato EstadoCandidato { get; set; }
        public Ternas Ternas { get; set; }
        public int TernasId { get; set; }
    }
}