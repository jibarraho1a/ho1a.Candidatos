using ho1a.reclutamiento.enums.Plazas;
using System;
using System.Collections.Generic;

namespace ho1a.reclutamiento.services.ViewModels.Plazas
{
    public class EntrevistaViewModel : BaseViewModel
    {
        public string Candidato { get; set; }
        public int CandidatoId { get; set; }
        public string Comentarios { get; set; }
        public List<CompetenciaViewModel> Competencias { get; set; }
        public string Debilidades { get; set; }
        public string Entrevistador { get; set; }
        public string EntrevistadorUserName { get; set; }
        public DateTime? FechaInicioEntrevista { get; set; }
        public DateTime? FechaTerminoEntrevista { get; set; }
        public string Fortalezas { get; set; }
        public bool? Recomendable { get; set; }
        public string TipoEntrevista { get; set; }
        public EEntrevista TipoEntrevistaId { get; set; }
    }
}
