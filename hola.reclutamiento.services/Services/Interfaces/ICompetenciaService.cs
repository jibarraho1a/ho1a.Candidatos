using ho1a.reclutamiento.models.Plazas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services.Interfaces
{
    public interface ICompetenciaService : IGeneralService<Competencia>
    {
        Task<List<TernaCandidato>> AddCompetencia(int idRequisicion, int idEntrevista, Competencia competenciaToEdit);
    }
}
