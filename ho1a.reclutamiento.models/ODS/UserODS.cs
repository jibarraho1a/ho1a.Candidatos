using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.enums.ODS;
using ho1a.reclutamiento.models.Catalogos;
using ho1a.reclutamiento.models.Seguridad;
using System.Collections.Generic;

namespace ho1a.reclutamiento.models.ODS
{
    public class UserODS : BaseUser
    {
        public List<Empresa> Empresas { get; set; }
        public List<User> Equipo { get; set; }
        public ENivelCompetencia? NivelCompetencia { get; set; }
    }
}
