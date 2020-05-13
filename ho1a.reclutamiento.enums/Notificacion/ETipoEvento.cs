using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ho1a.reclutamiento.enums.Notificacion
{
    public enum ETipoEvento
    {
        [Description("Alta Candidato")]
        AltaCandidato = 1,
        [Description("Invitar candidato")]
        InvitarCandidato,
        ResetearContrasenia,
        SolicitarAutorizacion,
        NotificarAceptacion,
        NotificarRechazo,
        NotificarCancelacion,
        NotificarAsignacion,
        PublicacionInterna,
        SolicitudDocumentosInicial,
        NotificacionTernaEntrevista,
        NotificacionRySEntrevista,
        AlertaSeguimientoTernas,
        SeleccionCandidato,
        SolicitudDocumentosFinal,
        PropuestaProfecionalEconomica,
        SolicitudAlta,
        NotificacionFirmaPropuesta,
        ConfirmacionAlta,
        CorreoCandidato,
        [Description("Notificar candidato capturar información inicial")]
        NotificarCandidatoCargaInicialInformacion,
        [Description("Notificar RH capturar información inicial")]
        NotificarRHCargaInicialInformacion,
        [Description("Notificar candidato capturar información complementaria")]
        NotificarCandidatoCargaComplementariaInformacion,
        [Description("Notificar RH capturar información complementaria")]
        NotificarRHCargaComplementariaInformacion,
        NotificarEntrevista,
        NotificarEntrevistaRS,
        NotificarSeleccionCandidato,
        EnviarOferta,
        NotificarAltaColaborador,
        NotificarConfirmacionAlta,
        CancelacionSolicitudLimiteTernas,
        NotificacionCandidatoEntrevista,
        [Description("Notificar al candidato fecha de Ingreso")]
        NotificarCandidatoFechaIngreso,
        [Description("Notificar al entrevistador fecha de la entrevista")]
        NotificarEntrevistaEntrevistador,
        [Description("Notifica al candidato que debe de cargas su documentación")]
        NotificarCandidatoCargaExpediente,
        [Description("Notifica a los Administradores de Expediente que se ha realizado la carga de Expediente")]
        NotificarRHCargaExpediente,
        [Description("Notifica al candidato que existe documentos con detalles")]
        NotificarCandidatoCorrecionExpediente,
        [Description("Notifica al candidato que debe de subir sus documentos para armar su expediente")]
        NotificarCandidatoAlta,
        [Description("Notifica carga de la propuesta")]
        NotificarCargaPropuesta,
        [Description("Notifica propuesta aceptada")]
        NotificarPropuestaAceptada
    }
}