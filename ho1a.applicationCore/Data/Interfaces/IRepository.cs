using ho1a.applicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ho1a.applicationCore.Data.Interfaces
{
    public interface IRepository<T>
        where T : BaseEntityId
    {
        T Add(T entity);
        void ApplyCurrentValues(T original, T current);
        void Delete(T entity);
        T GetById(int id);
        T GetSingleBySpec(ISpecification<T> spec);
        IEnumerable<T> List(ISpecification<T> spec);
        IEnumerable<T> ListAll();
        void Update(T entity);
    }
}
