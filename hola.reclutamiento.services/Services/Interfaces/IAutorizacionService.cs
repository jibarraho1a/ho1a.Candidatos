using ho1a.reclutamiento.enums.Plazas;
using ho1a.reclutamiento.models.Plazas;
using ho1a.reclutamiento.services.ViewModels.Requisicion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services.Interfaces
{
    public interface IAutorizacionService
    {
        Task<List<ValidacionesRequisicionViewModel>> AddValidacionAsync(
            int idRequisicion,
            string userNameRequerido,
            string userNameValidador,
            string motivoIngresoDescripcion,
            ENivelValidacion nivelValidacion);

        Task<List<ValidacionesRequisicionViewModel>> AprobacionAsync(
            int idRequisicion,
            string userName,
            string motivoIngreso,
            ValidacionesRequisicionViewModel validacion);

        Task AprobacionTopeSalarialAsync(
            int idRequisicion,
            string userName,
            ValidaRequisicion validacion);

        Task<List<ValidaRequisicion>> SetPermisosAsync(
            List<ValidacionesRequisicionViewModel> matrizAprobacion,
            string currentUserUserName,
            bool isAdmin = false);

        Task<List<ValidacionesRequisicionViewModel>> SolicitarAutorizacionAsync(
            int idRequisicion,
            string userName,
            string motivoIngreso);
    }
}
