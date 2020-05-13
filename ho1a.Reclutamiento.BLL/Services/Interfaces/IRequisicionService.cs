using ho1a.Reclutamiento.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ho1a.Reclutamiento.BLL.Services.Interfaces
{
    public interface IRequisicionService
    {
        int Agregar(Requisicion requisicion);
        void Actualizar(Requisicion requisicion);
        Requisicion Encontrar(int RequsicionId);
        IEnumerable<Requisicion> Encontrar();
        IEnumerable<Requisicion> EncontrarPorUsuarioAsignado();
        void Remover(int RequisicionId);
    }
}
