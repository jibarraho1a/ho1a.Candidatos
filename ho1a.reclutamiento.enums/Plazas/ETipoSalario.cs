using System.ComponentModel;

namespace ho1a.reclutamiento.enums.Plazas
{
    public enum ETipoSalario
    {
        [Description("Salario fijo mensual")]
        SalarioFijo = 1,
        [Description("Bonos")]
        Bonos,
        [Description("Comisiones")]
        Comisiones,
        [Description("Vales de despensa")]
        ValesDespensa,
        [Description("Caja de ahorro")]
        CajaAhorro,
        [Description("Fondo de ahorro")]
        FondoAhorro,
        [Description("Vales de comida")]
        ValesComida,
        [Description("PTU")]
        PTU,
        [Description("Otros ingresos")]
        OtrosIngresos
    }
}