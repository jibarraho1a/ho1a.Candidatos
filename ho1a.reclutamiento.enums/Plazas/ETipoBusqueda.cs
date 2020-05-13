using System.ComponentModel;

namespace ho1a.reclutamiento.enums.Plazas
{
    public enum ETipoBusqueda
    {
        SinDefinir = 0,
        [Description("Interna")]
        Interna = 1,
        [Description("Externa")]
        Externa
    }
}