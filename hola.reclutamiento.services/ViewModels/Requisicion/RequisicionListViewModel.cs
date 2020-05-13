namespace ho1a.reclutamiento.services.ViewModels.Requisicion
{
    public class RequisicionListViewModel : BaseViewModel
    {
        public bool CanDelete { get; set; }
        public string IdSolicitud => $"A{this.Id}";
        public string MotivoIngreso { get; set; }
        public string PuestoSolicitado { get; set; }
        public string Solicitante { get; set; }
        public string StatusMatriz { get; set; }
        public string StatusRequisicion { get; set; }
        public string Tabulador { get; set; }
        public string TipoPlaza { get; set; }
        public string Reclutador { get; set; }
        public string Localidad { get; set; }
        public string Area { get; set; }
        public string Direccion { get; set; }
        public string DiasSolicitud { get; internal set; }
    }
}
