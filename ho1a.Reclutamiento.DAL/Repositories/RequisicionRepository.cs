using Dapper;
using ho1a.Reclutamiento.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ho1a.Reclutamiento.DAL.Repositories
{
    internal class RequisicionRepository : RepositoryBase, IRequisicionRepository
    {
        public RequisicionRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public void Actualizar(Requisicion entidad)
        {
            Connection.Execute(
                "sp_UpdateRequisicions",
                transaction: Transaction
            );
        }

        public int Agregar(Requisicion entidad)
        {
            return Connection.ExecuteScalar<int>(
                "sp_InsertRequisicion",
                param: new { RequisicionId = entidad.Id, DescripcionTrabajo = entidad.DescripcionTrabajo },
                transaction: Transaction,
                commandType: CommandType.StoredProcedure
            );
        }

        public Requisicion Encontrar(int id)
        {
            return Connection.Query<Requisicion>(
                "SELECT * FROM dbo.Requisiciones WHERE id = @RequisicionId",
                param: new { RequisicionId = id },
                transaction: Transaction
            ).FirstOrDefault();
        }

        public void Remover(int id)
        {
            Connection.Execute(
              "DELETE FROM dbo.Requisiciones WHERE Id = @RequisicionId",
              param: new { RequisicionId = id },
              transaction: Transaction
          );
        }

        public IEnumerable<Requisicion> Todos()
        {
            return Connection.Query<Requisicion>(
                "SELECT * FROM dbo.Requisiciones",
                transaction: Transaction
            ).ToList();
        }
    }
}
