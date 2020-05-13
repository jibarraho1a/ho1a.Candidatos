using ho1a.reclutamiento.services.ViewModels.Candidato;

namespace ho1a.reclutamiento.services.ViewModels.Plazas
{
    public class ExpedienteViewModel : BaseViewModel
    {
        public string Descripcion { get; set; }
        public ExpedienteFileViewModel ExpedienteFile { get; set; }
        public string Nombre { get; set; }
        public bool Requerido { get; set; }
        public string TipoComponente { get; set; }
    }
}
