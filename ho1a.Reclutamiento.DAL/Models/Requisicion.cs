using System;
using System.Collections.Generic;
using System.Text;

namespace ho1a.Reclutamiento.DAL.Models
{
    public class Requisicion : ModelBase
    {
        public string DescripcionTrabajo { get; set; }
        public int EmpresaId { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime InicioReclutamiento { get; set; }
        public int LocalidadId { get; set; }
        public int TabuladorSalarioId { get; set; }
        public int MotivoIngresoId { get; set; }
        public int NumeroVacantes { get; set; }
        public string Observaciones { get; set; }
        public string OtraLocalidad { get; set; }      
        public int? PuestoSolicitadoId { get; set; }
        public string Responsabilidades { get; set; }
        public int? TipoPlazaId { get; set; }
        public string UserAsignado { get; set; }
        public string UserRequeridor { get; set; }
        public int VacantesSolicitadas { get; set; }
        public string Departamento { get; set; }
        public string Mercado { get; set; }
        public int TipoBusqueda { get; set; }
        public string NivelOrganizacional { get; set; }
        public string Alias { get; set; }
        public string AliasId { get; set; }
    }
}
