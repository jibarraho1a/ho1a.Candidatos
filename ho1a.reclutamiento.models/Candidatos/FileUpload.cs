using ho1a.applicationCore.Entities;
using System;

namespace ho1a.reclutamiento.models.Candidatos
{
    public class FileUpload : BaseEntityId
    {
        public DateTime Fecha { get; set; }
        public string FileName { get; set; }
        public string NombreOrigen { get; set; }
    }
}