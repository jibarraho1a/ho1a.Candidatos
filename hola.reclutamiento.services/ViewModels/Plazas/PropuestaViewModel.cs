using ho1a.reclutamiento.services.ViewModels.Candidato;
using System;
using System.Collections.Generic;

namespace ho1a.reclutamiento.services.ViewModels.Plazas
{
    public class PropuestaViewModel : BaseViewModel
    {
        public string Beneficios { get; set; }
        public decimal Bonos { get; set; }
        public string Comentarios { get; set; }
        public List<ExpedienteFileViewModel> Documentos { get; set; }
        public DateTime? FechaConfirmacionAlta { get; set; }
        public DateTime? FechaContestacion { get; set; }
        public DateTime? FechaEnvioPropuesta { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public DateTime? FechaNotificacionAlta { get; set; }
        public string Path { get; set; }
        public bool? PropuestaAceptada { get; set; }
        public decimal Salario { get; set; }
    }
}
