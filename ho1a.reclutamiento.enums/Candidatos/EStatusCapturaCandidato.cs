using System.ComponentModel;

namespace ho1a.reclutamiento.enums.Candidatos
{
    public enum EStatusCapturaCandidato
    {
        [Description("Candidato recien creado")]
        CandidatoNuevo = 0,
        [Description("Notificar captura inicial")]
        NotificarCapturaInicial = 1,
        [Description("Captura inicial")]
        CapturaInicial,
        [Description("Notificar captura complementaria")]
        NotificarCapturaComplementaria,
        [Description("Captura complementaria")]
        CapturaComplementaria
    }
}