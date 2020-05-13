using System.ComponentModel;

namespace ho1a.reclutamiento.enums.Candidatos
{
    public enum ETipoContratacion
    {
        [Description("No idoneo")]
        Planta = 1,
        [Description("No idoneo")]
        Proyecto,
        [Description("No idoneo")]
        Temporal1Mes,
        [Description("No idoneo")]
        Temporal3Meses,
        [Description("No idoneo")]
        Temporal6Meses
    }
}