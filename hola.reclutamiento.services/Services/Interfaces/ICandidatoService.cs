using ho1a.reclutamiento.enums.Notificacion;
using ho1a.reclutamiento.enums.Plazas;
using ho1a.reclutamiento.enums.Seguridad;
using ho1a.reclutamiento.models.Candidatos;
using ho1a.reclutamiento.models.Plazas;
using ho1a.reclutamiento.services.ViewModels.Plazas;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services.Interfaces
{
    public interface ICandidatoService : IGeneralService<Candidato>
    {
        Task<Candidato> AddCandidatoAsync(Candidato candidato);

        Task AddCandidatoToTernaAsync(int idRequisicion, int idCandidato, ETerna idTerna);

        Task<Requisicion> AlertarSeguimiento(int idRequisicion);

        Task<bool> ComplementoExpedienteAsync(int idCandidato, Candidato candidato);

        Task<Candidato> GetCandidatoByEmailAsync(string email);

        Task<Candidato> GetCandidatoByIdAsync(int idCandidato);

        Task<Candidato> GetCandidatoByIdAsync(Guid idCandidato);

        Task<Candidato> GetCandidatoIdoneoAsync(int idRequisicion);

        Task<List<Candidato>> GetCandidatosByIdRequisicionAsync(int idRequisicion);

        Task<IList<Entrevista>> GetEntrevistasAsync(int idRequisicion);

        Task<List<TernaCandidato>> GetTernaCandidatoAsync(int idRequisicion);

        Task<bool> NotificarAsync(int idCandidato, ETipoEvento tipoEvento);

        Task NotificarExpedienteToCandidato(int idRequisicion, int idCandidato, ComentarioViewModel comentario);

        Task NotificarExpedienteToRH(int idRequisicion, int idCandidato);

        Task NotificarExpedienteToRH(int idCandidato);

        Task ResetCandidatoIdoneoAsync(int idRequisicion);

        Task<bool> SetCargaInformacionAsync(int idCandidato, ECargaInformacionCandidato cargaInformacion);

        Task<Candidato> UpdateCandidatoAsync(int idCandidato, Candidato candidato);

        Task<bool> AddResumenCVToCandidatoAsync(int idCandidato, Candidato candidato);
    }
}
