using System.ComponentModel;

namespace ho1a.reclutamiento.enums.Plazas
{
    public enum ETipoPrestacion
    {
        [Description("Días de vacaciones")]
        DiasVacaciones = 1,
        [Description("Porcentaje de prima de vacaciones")]
        PorcentajePrimaVacaciones,
        [Description("Días de aguinaldo")]
        DiasAguinaldo,
        [Description("Otras prestaciones")]
        OtrasPrestaciones
    }
}