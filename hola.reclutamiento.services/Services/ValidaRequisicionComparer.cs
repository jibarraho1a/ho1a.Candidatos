using ho1a.reclutamiento.models.Plazas;
using System;
using System.Collections.Generic;

namespace ho1a.reclutamiento.services.Services
{
    public class ValidaRequisicionComparer : IEqualityComparer<ValidaRequisicion>
    {
        public bool Equals(ValidaRequisicion x, ValidaRequisicion y)
        {
            if (ReferenceEquals(x, y)) return true;

            return x != null && y != null && x.NivelValidacion.Equals(y.NivelValidacion)
                   && x.EstadoValidacion.Equals(y.EstadoValidacion) && x.AprobadorUserName.Equals(y.AprobadorUserName);
        }

        public int GetHashCode(ValidaRequisicion obj)
        {
            var hashAprobadorUserName = obj.AprobadorUserName == null ? 0 : obj.AprobadorUserName.GetHashCode();

            var hashNivelHashCode = obj.NivelValidacion.GetHashCode();

            var hashEstadoValidacion = obj.EstadoValidacion.GetHashCode();

            return hashAprobadorUserName ^ hashNivelHashCode ^ hashEstadoValidacion;
        }
    }
}