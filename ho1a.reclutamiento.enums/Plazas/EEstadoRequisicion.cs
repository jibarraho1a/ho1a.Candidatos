using System.ComponentModel;

namespace ho1a.reclutamiento.enums.Plazas
{
    public enum EEstadoRequisicion
    {
        [Description("Abierta")]
        Abierta = 0,
        [Description("En espera de autorización")]
        SolicitudAutorizacion = 1,
        [Description("Pendiente de autorizar")]
        PendienteAutorizacion,
        [Description("Autorizado")]
        Autorizado,
        [Description("Pendiente por reasignar")]
        PorReasignar,
        [Description("Busqueda de candidatos")]
        BusquedaCandidatos,
        [Description("Entrevista Pendiente")]
        EntrevistaPendiente,
        [Description("Entrevista por RH")]
        EntrevistaRH,
        [Description("Entrevista por Solicitante")]
        EntrevistaSolicitante,
        [Description("Decisión")]
        Decision,
        [Description("Oferta")]
        Oferta,
        [Description("Alta")]
        Alta,
        [Description("Contratado")]
        Contratado,
        [Description("Expediente incompleto")]
        ExpedienteIncompleto,
        [Description("Cerrada")]
        Cerrado,
        [Description("Cancelada")]
        Cancelada,
        [Description("Rechazada")]
        Rechazada,
        [Description("Reasignada")]
        Reasignada
    }
}
