using ho1a.reclutamiento.enums.Plazas;
using System.Collections.Generic;

namespace ho1a.reclutamiento.services.ViewModels.Plazas
{
    public class ComponenteViewModel : BaseViewModel
    {
        public List<AccionViewModel> Acciones { get; set; }
        public List<ComponenteViewModel> Componentes { get; set; }
        public ComponenteViewModel ComponenteView { get; set; }
        public int? ComponenteViewId { get; set; }
        public bool CurrentStep { get; set; }
        public string Descripcion { get; set; }
        public bool Edicion { get; set; }
        public int Index { get; set; }
        public string Nombre { get; set; }
        public EEstadoValidacion StateValidation { get; set; }
        public List<ValidacionViewModel> Validaciones { get; set; }
        public bool Visible { get; set; }
    }
}
