using ho1a.reclutamiento.enums.Plazas;
using System;

namespace ho1a.reclutamiento.services.ViewModels.Requisicion
{
    public class ValidacionesRequisicionViewModel
    {
        public bool Active { get; set; }
        public bool CanApprove { get; set; }
        public bool CanCancel { get; set; }
        public bool CanDeny { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public string DescriptionRequired { get; set; }
        public int Id { get; set; }
        public string Info { get; set; }
        public string Name { get; set; }
        public ENivelValidacion NivelValidacion { get; set; }
        public EEstadoValidacion StateValidation { get; set; }
        public string Timelapse { get; set; }
        public string UserName { get; set; }
    }
}
