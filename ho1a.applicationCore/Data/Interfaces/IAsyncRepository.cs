using ho1a.applicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ho1a.applicationCore.Data.Interfaces
{
    public interface IAsyncRepository<T>
        where T : BaseEntityId
    {
        Task<T> AddAsync(T entity);
        void ApplyCurrentValues(T original, T current);
        Task DeleteAsync(T entity);
        Task<T> GetByIdAsync(int id);
        Task<T> Single(ISpecification<T> spec);
        Task<List<T>> ListAllAsync();
        Task<List<T>> ListAsync(ISpecification<T> spec);
        Task UpdateAsync(T entity);
    }
}
