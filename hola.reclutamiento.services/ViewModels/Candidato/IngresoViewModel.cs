namespace ho1a.reclutamiento.services.ViewModels.Candidato
{
    public class IngresoViewModel : BaseViewModel
    {
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public string Nombre { get; set; }
        public int? TipoIngresoId { get; set; }
        public int? UltimoTrabajoId { get; set; }
    }
}
