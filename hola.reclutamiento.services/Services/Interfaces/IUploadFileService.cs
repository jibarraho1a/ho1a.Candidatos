using ho1a.reclutamiento.models.Candidatos;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services.Interfaces
{
    public interface IUploadFileService
    {
        Task<FileUpload> AddExpedienteArchivo(
            int idRequisicion,
            int candidatoId,
            int idExpediente,
            FileUpload fileUpload,
            int? idExpedienteArchivo = null);

        Task<FileUpload> AddRequisicionArchivo(int idRequisicion, int idExpediente, FileUpload fileUpload);

        Task<bool> DeleteExpedienteArchivo(int idRequisicion, int idCandidato, int idExpediente);
    }
}
