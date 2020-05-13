using ho1a.reclutamiento.enums.Candidatos;
using ho1a.reclutamiento.models.Candidatos;
using ho1a.reclutamiento.enums.Plazas;
using ho1a.reclutamiento.models.Plazas;
using ho1a.reclutamiento.models.Seguridad;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services.Interfaces
{
    public interface IRequisicionService : IGeneralService<Requisicion>
    {
        Task AddCandidatoAsync(int idRequisicion, List<Candidato> candidatos, bool isNew = false);

        Task ConfirmarAlta(int idRequisicion, List<Attachment> attachments);

        Task ContestarPropuestaAsync(int idRequisicion, bool aceptaPropuesta, string comentario);

        Task EnviarPropuestaAsync(int idRequisicion, List<Attachment> attachments);

        Task EstablecerFechaIngresoAsync(int idRequisicion, DateTime fechaIngreso);

        Task EstablecerSalarioPropuesto(int idRequisicion, dynamic propuesta);

        Task EstablecerUsuarioExpediente(int idRequisicion, string userAd);

        Task<List<Ternas>> GetTernasByRequisicionAsync(int idRequisicion);

        Task NotificarAltaAsync(int idRequisicion);

        Task<User> SetAsignacionAsync(
            Requisicion requisicion,
            string userAsignacion = null,
            bool estableceInicioReclutamiento = true);

        Task<User> SetAsignacionAsync(
            int idRequisicion,
            string userAsignacion = null,
            bool estableceInicioReclutamiento = true);

        Task SetCandidatoIdoneo(int idRequisicion, int idCandidato, EEstadoCandidato valor);

        Task SetInicioRequisicionAsync(int idRequisicion);

        Task SetTipoBusqueda(int idRequisicion, ETipoBusqueda idTipoBusqueda);
    }
}
