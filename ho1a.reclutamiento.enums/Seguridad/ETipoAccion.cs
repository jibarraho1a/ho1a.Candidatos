using System.ComponentModel;

namespace ho1a.reclutamiento.enums.Seguridad
{
    public enum ETipoAccion
    {
        [Description("Aceptar")]
        Aceptar = 1,
        [Description("Cancelar")]
        Cancelar,
        [Description("Guardar")]
        Guardar,
        [Description("Imprimir")]
        Imprimir,
        [Description("Nuva solicitud")]
        SolicitarRequisicion,
        [Description("Autorizar solicitud")]
        Autorizar,
        [Description("Rechazar solicitud")]
        Denegar,
        [Description("Rechazar")]
        Rechazar,
        [Description("Confirmar")]
        Confirmar,
        [Description("Buscar candidato")]
        BuscarCandidato,
        [Description("Nuevo candidato")]
        NuevoCandidato,
        [Description("Asignar terna")]
        AsignarTerna,
        [Description("Asignar fecha entrevista 1")]
        AsignarFechasEntrevista1,
        [Description("Asignar fecha entrevista 2")]
        AsignarFechasEntrevista2,
        [Description("Asignar fecha entrevista invitado")]
        AsignarFechasEntrevista3,
        [Description("Establecer candidato idoneo")]
        EstablecerCandidatoIdoneo,
        [Description("Establecer liderazgo")]
        EstablecerLiderazgo,
        [Description("Establecer conocimiento")]
        EstablecerConocimiento
    }
}