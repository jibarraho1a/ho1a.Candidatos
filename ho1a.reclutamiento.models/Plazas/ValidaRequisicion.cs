using ho1a.reclutamiento.models.Seguridad;

namespace ho1a.reclutamiento.models.Plazas
{
    public class ValidaRequisicion : ValidacionRequisicionPlaza
    {
        public Requisicion Requisicion { get; set; }
        public int RequisicionId { get; set; }
        public new User UserValidador { get; set; }
    }
}