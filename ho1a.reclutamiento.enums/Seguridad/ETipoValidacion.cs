using System.ComponentModel;

namespace ho1a.reclutamiento.enums.Seguridad
{
    public enum ETipoValidacion
    {
        [Description("required")]
        Required = 1,
        [Description("min")]
        Min,
        [Description("max")]
        Max,
        [Description("email")]
        Email,
        [Description("maxLength")]
        MaxLength,
        [Description("minLength")]
        MinLength,
        [Description("nullValidator")]
        NullValidator,
        [Description("pattern")]
        Pattern,
        [Description("requiredTrue")]
        RequiredTrue
    }
}