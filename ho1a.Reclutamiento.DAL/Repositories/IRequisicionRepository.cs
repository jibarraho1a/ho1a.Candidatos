using ho1a.Reclutamiento.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ho1a.Reclutamiento.DAL.Repositories
{
    public interface IRequisicionRepository
    {
        int Agregar(Requisicion entidad);
        IEnumerable<Requisicion> Todos();
        Requisicion Encontrar(int id);
        void Remover(int id);
        void Actualizar(Requisicion entidad);
    }
}
