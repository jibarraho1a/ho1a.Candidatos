using System.ComponentModel;

namespace ho1a.reclutamiento.enums.Catalogos
{
    public enum ETipoArchivo
    {
        [Description("Documentos del candidato")]
        CapturaCandidato = 1,
        [Description("Documentos complementarios del candidato")]
        CapturaComplementariaCandidato,
        [Description("Documentos de propuesta")]
        Propuesta,
        [Description("Documentos del expediente del colaborador")]
        Expediente,
        [Description("Documentos del administrador expediente")]
        AdministradorExpediente
    }
}