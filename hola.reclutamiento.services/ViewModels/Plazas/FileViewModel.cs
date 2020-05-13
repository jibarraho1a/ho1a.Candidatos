using System;

namespace ho1a.reclutamiento.services.ViewModels.Plazas
{
    public class FileViewModel : BaseViewModel
    {
        public DateTime FechaActualizacion { get; set; }
        public object IdRequisicion { get; set; }
        public string NombreDestino { get; set; }
        public string NombreOrigen { get; set; }
        public object RequisicionId { get; set; }
        public string Url { get; set; }
    }
}
