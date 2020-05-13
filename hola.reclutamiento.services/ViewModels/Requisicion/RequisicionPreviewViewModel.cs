using ho1a.reclutamiento.models.Plazas;
using ho1a.reclutamiento.services.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace ho1a.reclutamiento.services.ViewModels.Requisicion
{
    public class RequisicionPreviewViewModel : BaseViewModel
    {
        public string Colonia { get; set; }

        public string CP { get; set; }

        public string Departamento { get; set; }

        public string Domicilio { get; set; }

        public string Empresa { get; set; }

        public string Estado { get; set; }

        public string EstadoCivil { get; set; }

        public DateTime? FechaIngreso { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public string JefeInmediato { get; set; }

        public string Localidad { get; set; }

        public string Materno { get; set; }

        public string Municipio { get; set; }

        public int NoColaborador { get; set; }

        public string Nombre { get; set; }

        public string Observaciones { get; set; }

        public string Paterno { get; set; }

        public bool? ProcesoAltaCompleta { get; set; }

        public string Puesto { get; set; }

        public string RFC { get; set; }

        public List<TareasAlta> Tareas { get; set; }

        public SelectListItem TipoContrato { get; set; }
    }
}
