using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.enums.Plazas;
using ho1a.reclutamiento.models.Candidatos;
using System;
using System.Collections.Generic;

namespace ho1a.reclutamiento.models.Plazas
{
    public class Entrevista : BaseEntityId
    {
        public Entrevista()
        {
            this.Active = true;
        }
        public Candidato Candidato { get; set; }
        public int? CandidatoId { get; set; }
        public string Comentarios { get; set; }
        public ICollection<Competencia> Competencias { get; set; }
        public string Debilidades { get; set; }
        public string Entrevistador { get; set; }
        public DateTime? FechaInicioEntrevista { get; set; }
        public DateTime? FechaTerminoEntrevista { get; set; }
        public string Fortalezas { get; set; }
        public bool? Recomendable { get; set; }
        public EEntrevista TipoEntrevista { get; set; }
    }
}