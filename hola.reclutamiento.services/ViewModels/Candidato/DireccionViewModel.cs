
namespace ho1a.reclutamiento.services.ViewModels.Candidato
{
    public class DireccionViewModel : BaseViewModel
    {
        public string Calle { get; set; }
        public int? CandidatoId { get; set; }
        public string CodigoPostal { get; set; }
        public string Colonia { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
    }
}
