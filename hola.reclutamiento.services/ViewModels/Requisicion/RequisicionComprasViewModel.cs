namespace ho1a.reclutamiento.services.ViewModels.Requisicion
{
    public class RequisicionComprasViewModel : RequisicionViewModelBase
    {
        public decimal? Monto { get; set; }
        public int RequisicionId { get; set; }
        public string TipoRequisicion { get; set; }
        public string UserAdministracion { get; set; }
        public string UserComprador { get; set; }
        public string UserDireccionGeneral { get; set; }
    }
}
