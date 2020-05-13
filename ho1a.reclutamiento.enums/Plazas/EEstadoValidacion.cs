using System.ComponentModel;

namespace ho1a.reclutamiento.enums.Plazas
{
    public enum EEstadoValidacion
    {
        [Description("Pendiente de autorización")]
        Pendiente = -1,
        [Description("Pendiente de autorización")]
        Actual = 0,
        [Description("Autorizada")]
        Aprobada = 1,
        [Description("Rechazada")]
        Rechazada = 2,
        [Description("Cancelada")]
        Cancelado = 3,
        [Description("Reasignada")]
        Reasignada = 4
    }
}