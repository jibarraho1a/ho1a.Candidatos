using ho1a.applicationCore.Data.Interfaces;
using ho1a.reclutamiento.models.Plazas;
using ho1a.reclutamiento.services.Data.Interfaces;
using ho1a.reclutamiento.services.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services
{
    public class CompetenciaService : GeneralService<Competencia>, ICompetenciaService
    {
        private readonly IAsyncRepository<Competencia> competenciaRepository;

        public CompetenciaService(
            IAsyncRepository<Competencia> competenciaAsyncRepository,
            IRepository<Competencia> competenciaRepository)
            : base(competenciaAsyncRepository, competenciaRepository)
        {
            this.competenciaRepository = competenciaAsyncRepository;
        }

        public async Task<List<TernaCandidato>> AddCompetencia(
            int idRequisicion,
            int idEntrevista,
            Competencia liderazgo)
        {
            var result = await this.competenciaRepository.AddAsync(liderazgo)
                                   .ConfigureAwait(false);

            return null;
        }
    }
}
