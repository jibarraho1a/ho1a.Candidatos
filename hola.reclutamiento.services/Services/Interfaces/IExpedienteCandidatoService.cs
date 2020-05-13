using ho1a.reclutamiento.models.Catalogos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services.Interfaces
{
    public interface IExpedienteCandidatoService
    {
        Task<List<Expediente>> GetDocumentosCandidato(int idCandidato);

        Task<List<Expediente>> GetDocumentosPropuesta(int idRequisicion, int idCandidato);

        Task<List<Expediente>> GetExpedienteAdministrador(int idCandidato);

        Task<List<Expediente>> GetExpedienteCandidato(int idCandidato);
    }
}
