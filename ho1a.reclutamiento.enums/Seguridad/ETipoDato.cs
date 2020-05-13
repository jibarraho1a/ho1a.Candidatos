using System.ComponentModel;

namespace ho1a.reclutamiento.enums.Seguridad
{
    public enum ETipoDato
    {
        [Description("date")]
        Date = 1,
        [Description("datefull")]
        DateFull,
        [Description("currency")]
        Currency,
        [Description("string")]
        String,
        [Description("link")]
        Link,
        [Description("number")]
        Number,
        [Description("boolean")]
        Boolean
    }
}