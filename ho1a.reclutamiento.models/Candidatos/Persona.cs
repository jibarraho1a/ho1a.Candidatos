using ho1a.applicationCore.Entities;

namespace ho1a.reclutamiento.models.Candidatos
{
    public class Persona : BaseEntityId
    {
        public string Email { get; set; }
        public string Materno { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
    }
}