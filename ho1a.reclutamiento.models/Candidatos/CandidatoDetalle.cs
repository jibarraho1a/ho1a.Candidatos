using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.enums.Candidatos;
using ho1a.reclutamiento.models.Catalogos;
using ho1a.reclutamiento.models.Plazas;
using System;
using System.Collections.Generic;

namespace ho1a.reclutamiento.models.Candidatos
{
    public class CandidatoDetalle : BaseEntityId
    {
        public CandidatoDetalle()
        {
            CandidatoExpediente = new CandidatoExpediente();
            ReferenciasLaborales = new List<ReferenciaLaboral>();
            ReferenciasPersonales = new List<ReferenciaPersonal>();
            UltimoTrabajo = new UltimoTrabajo();
        }
        public Candidato Candidato { get; set; }
        public CandidatoExpediente CandidatoExpediente { get; set; }
        public int CandidatoId { get; set; }
        public DateTime? CargaInformacion1 { get; set; }
        public DateTime? CargaInformacion2 { get; set; }
        public bool? Certificacion { get; set; }
        public string Curp { get; set; }
        public FileUpload Cv { get; set; }
        public Direccion Direccion { get; set; }
        public EstadoCivil EstadoCivil { get; set; }
        public int? EstadoCivilId { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string LinkedIn { get; set; }
        public int? LocalidadSucursalId { get; set; }
        public string LugarNacimiento { get; set; }
        public string Nss { get; set; }
        public string Observaciones { get; set; }
        public string OtraReferenciaVacante { get; set; }
        public decimal? PretencionEconomica { get; set; }
        public bool? ProcesoAltaCompleta { get; set; }
        public PuestoSolicitado PuestoSolicitado { get; set; }
        public int? PuestoSolicitadoId { get; set; }
        public string RecomendadoPor { get; set; }
        public ICollection<ReferenciaLaboral> ReferenciasLaborales { get; set; }
        public ICollection<ReferenciaPersonal> ReferenciasPersonales { get; set; }
        public ReferenciaVacante ReferenciaVacante { get; set; }
        public int? ReferenciaVacanteId { get; set; }
        public Requisicion Requisicion { get; set; }
        public int? RequisicionId { get; set; }
        public string Rfc { get; set; }
        public EStatusCapturaCandidato StatusCapturaInformacion { get; set; }
        public EEstadoCandidato StatusSeleccion { get; set; }
        public decimal? SueldoFijoMensual { get; set; }
        public string TelefonoCasa { get; set; }
        public string TelefonoCelular { get; set; }
        public ETipoContratacion TipoContratacion { get; set; }
        public Salario UltimoSalario { get; set; }
        public int? UltimoSalarioId { get; set; }
        public UltimoTrabajo UltimoTrabajo { get; set; }
        public int Experiencia { get; set; }
    }
}