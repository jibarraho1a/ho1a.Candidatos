using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.ViewModels.Candidato
{
    public class ExpedienteFileViewModel : BaseViewModel
    {
        public string File { get; set; }
        public bool Loaded { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool ReadOnly { get; set; }
        public bool Required { get; set; }
        public string Title { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
