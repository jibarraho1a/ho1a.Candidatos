using ho1a.applicationCore.Entities;

namespace ho1a.reclutamiento.models.Seguridad
{
    public class Suplantar : BaseEntityId
    {
        public string UserLogin { get; set; }
        public string UserProfile { get; set; }
    }
}