using System.ComponentModel;

namespace ho1a.reclutamiento.enums.Plazas
{
    public enum ENivelValidacion
    {
        [Description("Requeridor")]
        Requeridor = 1,
        [Description("Presupuesto")]
        Presupuesto,
        [Description("Director de Área")]
        DireccionArea,
        [Description("Dirección RH")]
        DireccionRH,
        [Description("Dirección de Operaciones")]
        DireccionGeneral,
        [Description("Coordinador de Reclutamiento y Selección")]
        CoordinadorReclutamientoSeleccion,
        [Description("Tope salarial Dirección RH")]
        TopeSalarialDireccionRH,
        [Description("Tope salarial Dirección de Operaciones")]
        TopeSalarialDireccionGeneral
    }
}