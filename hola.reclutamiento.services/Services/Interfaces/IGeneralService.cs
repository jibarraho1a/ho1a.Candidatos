using ho1a.applicationCore.Data.Interfaces;
using ho1a.reclutamiento.services.Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services.Interfaces
{
    public interface IGeneralService<T>
    {
        ISpecification<T> AllSpecification { get; set; }

        ISpecification<T> ByIdSpecification { get; set; }

        Task<T> AddAsync(T item);

        Task<bool> DeleteAsync(ISpecification<T> specification);

        Task<bool> DeleteAsync(int id);

        Task<List<T>> GetAsync();

        Task<List<T>> GetAsync(ISpecification<T> specification);

        Task<T> GetAsync(int id);

        T Single(ISpecification<T> specification);

        Task<T> UpdateAsync(int id, T item);
    }
}
