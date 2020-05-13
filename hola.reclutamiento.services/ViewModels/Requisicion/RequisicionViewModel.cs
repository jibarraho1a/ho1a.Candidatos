using ho1a.applicationCore.Utilerias;
using ho1a.reclutamiento.enums.Plazas;
using ho1a.reclutamiento.models.Catalogos;
using ho1a.reclutamiento.models.Seguridad;
using ho1a.reclutamiento.services.ViewModels.Plazas;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ho1a.reclutamiento.services.ViewModels.Requisicion
{
    public class RequisicionViewModel : BaseViewModel
    {
        public string Alias { get; set; }
        public string AliasId { get; set; }
        public User Asignado { get; set; }
        public string AsignadoUserName { get; set; }
        public string Comentarios { get; set; }
        public string Departamento { get; set; }
        public string DescripcionTrabajo { get; set; }
        public SelectListItem Empresa { get; set; }
        public IEnumerable<SelectListItem> Empresas { get; set; }
        public DateTime? FechaFin { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaSolicitud { get; set; }
        public DateTime? InicioReclutamiento { get; set; }
        public SelectListItem Localidad { get; set; }
        public IEnumerable<SelectListItem> Localidades { get; set; }
        public IEnumerable<ValidacionesRequisicionViewModel> MatrizValidacion { get; set; }
        public string Mercado { get; set; }
        public SelectListItem MotivoIngreso { get; set; }
        public IEnumerable<SelectListItem> MotivosIngresos { get; set; }
        public string NivelOrganizacional { get; set; }
        public int NumeroVacantes { get; set; }
        public string Observaciones { get; set; }
        public string OtraLocalidad { get; set; }
        public PropuestaViewModel Propuesta { get; set; }
        public IEnumerable<SelectListItem> Puestos { get; set; }
        public SelectListItem PuestoSolicitado { get; set; }
        public int? PuestoSolicitadoId { get; set; }
        public string Responsabilidades { get; set; }
        public User Solicitante { get; set; }
        public string SolicitanteUserName { get; set; }
        public ValidacionesRequisicionViewModel StatusMatriz =>
            this.MatrizValidacion?.FirstOrDefault(v => v?.StateValidation == EEstadoValidacion.Actual);
        public TabuladorSalario TabuladorSalario { get; set; }
        public int? TabuladorSalarioId { get; set; }
        public string TabuladorSalarioMonto { get; set; }
        public ETipoBusqueda TipoBusqueda { get; set; }
        public SelectListItem TipoPlaza { get; set; }
        public IEnumerable<SelectListItem> TiposPlazas { get; set; }
        public int VacantesSolicitadas { get; set; }
        public DateTime? FechaUltimoEstatus { get; set; }
        public string StatusRequisicion = EEstadoRequisicion.Autorizado.GetDescription();
    }
}