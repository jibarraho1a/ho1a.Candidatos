using ho1a.reclutamiento.models.Plazas;
using ho1a.reclutamiento.services.ViewModels.Plazas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services.Interfaces
{
    public interface IEntrevistaService : IGeneralService<Entrevista>
    {
        Task<List<Ternas>> AddCompetencia(int idRequisicion, int idEntrevista, Competencia competenciaToEdit);

        Task AddCompetenciasToEntrevistaFromPlantilla(int idRequisicion, int idEntrevista);

        Task AddEntrevistadorInvitado(int idRequisicion, int idCandidato, Entrevista entrevista);

        Task<List<Ternas>> GetEntrevistasByRequisicionAsync(int idRequisicion);

        Task<List<Ternas>> SetDateByRequisicionAndEntrevistaAsync(
            int idRequisicion,
            int idEntrevista,
            EntrevistaViewModel entrevista);
    }
}
