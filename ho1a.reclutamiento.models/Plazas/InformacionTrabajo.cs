using ho1a.reclutamiento.models.Catalogos;

namespace ho1a.reclutamiento.models.Plazas
{
    public class InformacionTrabajo
    {
        public string Departamento { get; set; }
        public string Descripcion { get; set; }
        public string Mercado { get; set; }
        public string NivelOrganizacional { get; set; }
        public int NumeroVacantes { get; set; }
        public string PuestoSolicitado { get; set; }
        public string Responsabilidades { get; set; }
        public TabuladorSalario Tabulador { get; set; }
    }
}