using ho1a.applicationCore.Entities;

namespace ho1a.reclutamiento.models.Plazas
{
    public class Competencia : BaseEntityId
    {
        public string Descripcion { get; set; }
        public Entrevista Entrevista { get; set; }
        public string Nombre { get; set; }
        public PlantillaEntrevista PlantillaEntrevista { get; set; }
        public decimal Resultado { get; set; }
    }
}