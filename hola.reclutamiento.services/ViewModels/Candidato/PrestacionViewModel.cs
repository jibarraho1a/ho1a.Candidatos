namespace ho1a.reclutamiento.services.ViewModels.Candidato
{
    public class PrestacionViewModel : BaseViewModel
    {
        public string Descripcion { get; set; }
        public string Nombre { get; set; }
        public int? TipoPrestacionId { get; set; }
        public int? UltimoTrabajoId { get; set; }
        public string Valor { get; set; }
    }
}
