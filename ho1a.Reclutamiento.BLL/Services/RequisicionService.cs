using ho1a.Reclutamiento.BLL.Services.Interfaces;
using ho1a.Reclutamiento.DAL;
using ho1a.Reclutamiento.DAL.Models;
using System;
using System.Collections.Generic;

namespace ho1a.Reclutamiento.BLL
{
    public class RequisicionService : IRequisicionService
    {
        IUnitOfWork _unitOfWork;
        public RequisicionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Actualizar(Requisicion requisicion)
        {
            using (_unitOfWork)
            {
                _unitOfWork.RequisicionRepository.Actualizar(requisicion);
                _unitOfWork.Commit();
            }
        }

        public int Agregar(Requisicion requisicion)
        {
            using (_unitOfWork)
            {
                int newRequisicionId = _unitOfWork.RequisicionRepository.Agregar(requisicion);
                _unitOfWork.Commit();
                return newRequisicionId;
            }
        }

        public Requisicion Encontrar(int requsicionId)
        {
            return _unitOfWork.RequisicionRepository.Encontrar(requsicionId);
        }

        public IEnumerable<Requisicion> Encontrar()
        {
            return _unitOfWork.RequisicionRepository.Todos();
        }

        public IEnumerable<Requisicion> EncontrarPorUsuarioAsignado()
        {
            throw new NotImplementedException();
        }

        public void Remover(int requisicionId)
        {
            _unitOfWork.RequisicionRepository.Remover(requisicionId);
        }
    }
}
