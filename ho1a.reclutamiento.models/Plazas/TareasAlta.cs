using ho1a.reclutamiento.models.Seguridad;
using System;
using System.Collections.Generic;
using System.Text;

namespace ho1a.reclutamiento.models.Plazas
{
    public class TareasAlta
    {
        public TareasAlta()
        {
            this.Actividades = new List<string>();
            this.Usuarios = new List<User>();
        }

        public List<string> Actividades { get; set; }

        public List<User> Usuarios { get; set; }
    }
}
