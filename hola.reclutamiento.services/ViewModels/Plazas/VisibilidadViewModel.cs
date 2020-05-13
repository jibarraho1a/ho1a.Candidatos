using System.Collections.Generic;

namespace ho1a.reclutamiento.services.ViewModels.Plazas
{
    public class VisibilidadViewModel : BaseViewModel
    {
        public List<ComponenteViewModel> Componente { get; set; }
        public List<ComponenteViewModel> Componentes { get; set; }
        public string Descripcion { get; set; }
        public string Vista { get; set; }
    }
}
