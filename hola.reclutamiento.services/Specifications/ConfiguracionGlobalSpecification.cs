using ho1a.reclutamiento.models.Configuracion;

namespace ho1a.reclutamiento.services.Specifications
{
    public class ConfiguracionGlobalSpecification : BaseSpecification<Configuracion>
    {
        public ConfiguracionGlobalSpecification()
            : base(a => a.Active)
        {
        }
    }
}
