using System.ComponentModel;

namespace ho1a.reclutamiento.enums.Candidatos
{
    public enum EEstadoCandidato
    {
        [Description("Rechazó propuesta")]
        RechazaPropuesta = -5,
        [Description("No interesado")]
        NoInteresado = -4,
        [Description("En duda")]
        EnDuda = -3,
        [Description("Descartado")]
        Descartado = -2,
        [Description("No idoneo")]
        NoIdoneo = -1,
        [Description("No calificado")]
        NoCalificado,
        [Description("Idoneo")]
        Idoneo,
        [Description("Acepta propuesta")]
        AceptaPropuesta,
        [Description("Colaborador o Contratado")]
        Colaborador
    }
}