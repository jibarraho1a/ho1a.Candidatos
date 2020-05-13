using ho1a.Reclutamiento.DAL.Repositories;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ho1a.Reclutamiento.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private IRequisicionRepository _requisicionRepository;

        private bool _disposed;

        public UnitOfWork()
        {
            _connection = new SqlConnection("Server=H1DEV1GDL;Initial Catalog=ReclutamientoDB;Integrated Security=true;");
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public IRequisicionRepository RequisicionRepository
        {
            get { return _requisicionRepository ?? (_requisicionRepository = new Lazy<RequisicionRepository>(() => new RequisicionRepository(_transaction)).Value); }
        }


        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                resetRepositories();
            }
        }

        private void resetRepositories()
        {
            _requisicionRepository = null;
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        private void dispose(bool disposing)
        {
            if (!_disposed)
            {
                if(disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if(_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            dispose(false);
        }
    }
}
