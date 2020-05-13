using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.enums.ODS;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ho1a.reclutamiento.models.Seguridad
{
    public class User : BaseUser
    {
        [NotMapped]
        public CandidatoUser CandidatoUser { get; set; }
        public List<User> Equipo { get; set; }
        public ENivelCompetencia? NivelCompetencia { get; set; }
        public string Responsabilidad { get; set; }
        public string Responsabilidades { get; set; }
        public bool ShowCatalog { get; set; }
        public bool ShowExpediente { get; set; }
        public bool ShowNew { get; set; }
        public bool ShowSearch { get; set; }
        public JsonWebToken Token { get; set; }
        public override string ToString() => $"{this.Nombre} {this.Apellidos}";
    }
}