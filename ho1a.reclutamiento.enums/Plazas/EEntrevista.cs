using System.ComponentModel;

namespace ho1a.reclutamiento.enums.Plazas
{
    public enum EEntrevista
    {
        [Description("Entrevista #1")]
        Entrevista1 = 1,
        [Description("Entrevista #2")]
        Entrevista2,
        [Description("Entrevista invitado")]
        Invitado
    }
}