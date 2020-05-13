
namespace ho1a.reclutamiento.services.ViewModels.Candidato
{
    public class CandidatoBusquedaViewModel : BaseViewModel
    {
        public string Email { get; set; }
        public string Materno { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Puesto { get; set; }
        public int? PuestoId { get; set; }
        public string Status { get; set; }
    }
}