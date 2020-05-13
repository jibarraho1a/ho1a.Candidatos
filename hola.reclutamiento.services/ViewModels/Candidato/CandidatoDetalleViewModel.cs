using ho1a.reclutamiento.enums.Candidatos;
using System;
using System.Collections.Generic;

namespace ho1a.reclutamiento.services.ViewModels.Candidato
{
    public class CandidatoDetalleViewModel : BaseViewModel
    {
        public int CandidatoId { get; set; }
        public bool? Certificacion { get; set; }
        public string Curp { get; set; }
        public DireccionViewModel Direccion { get; set; }
        public List<ExpedienteFileViewModel> Documentos { get; set; }
        public EstadoCivilViewModel EstadoCivil { get; set; }
        public string EstadoCivilId { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string LinkedIn { get; set; }
        public string LocalidadSucursalId { get; set; }
        public string LugarNacimiento { get; set; }
        public string Nss { get; set; }
        public string OtraReferenciaVacante { get; set; }
        public decimal? PretencionEconomica { get; set; }
        public string PuestoSolicitado { get; set; }
        public string PuestoSolicitadoId { get; set; }
        public string RecomendadoPor { get; set; }
        public List<ReferenciaLaboralViewModel> ReferenciasLaborales { get; set; }
        public List<ReferenciaPersonalViewModel> ReferenciasPersonales { get; set; }
        public ReferenciaVacanteViewModel ReferenciaVacante { get; set; }
        public string ReferenciaVacanteId { get; set; }
        public string Rfc { get; set; }
        public EStatusCapturaCandidato StatusCapturaInformacion { get; set; }
        public EEstadoCandidato StatusSeleccion { get; set; }
        public string TelefonoCasa { get; set; }
        public string TelefonoCelular { get; set; }
        public SalarioViewModel UltimoSalario { get; set; }
        public string UltimoSalarioId { get; set; }
        public UltimoTrabajoViewModel UltimoTrabajo { get; set; }
        public string UltimoSalarioDescripcion { get; set; }
        public int Experiencia { get; set; }
        public ExpedienteFileViewModel Cv { get; set; }
    }
}
