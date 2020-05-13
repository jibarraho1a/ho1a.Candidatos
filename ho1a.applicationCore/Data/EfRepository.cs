using ho1a.applicationCore.Data.Enum;
using ho1a.applicationCore.Data.Interfaces;
using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.enums.ODS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ho1a.applicationCore.Data
{
    public class EfRepository<T> : IRepository<T>, IAsyncRepository<T>
        where T : BaseEntityId
    {
        private readonly DbContext dbContext;
        public EfRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public T Add(T entity)
        {
            this.dbContext.Set<T>()
                .Add(entity);
            this.dbContext.SaveChanges();

            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            this.dbContext.Set<T>()
                .Add(entity);
            await this.dbContext.SaveChangesAsync()
                .ConfigureAwait(true);

            return entity;
        }

        public void ApplyCurrentValues(T original, T current)
        {
            this.dbContext.Entry(original)
                .CurrentValues.SetValues(current);
        }

        public void Delete(T entity)
        {
            this.dbContext.Set<T>()
                .Remove(entity);
            this.dbContext.SaveChanges();
        }

        public Task DeleteAsync(T entity)
        {
            this.dbContext?.Set<T>()
                ?.Remove(entity);
            return this.dbContext?.SaveChangesAsync();
        }

        public void Dispose()
        {
            // var context = this.dbContext;
            // if (context != null)
            // {
            // context.Dispose();
            // }
        }

        public virtual T GetById(int id)
        {
            return this.dbContext?.Set<T>()
                ?.Find(id);
        }

        public async virtual Task<T> GetByIdAsync(int id)
        {
            return await this.dbContext.Set<T>().FindAsync(id)
                .ConfigureAwait(true);
        }

        public T GetSingleBySpec(ISpecification<T> spec)
        {
            return this.List(spec)
                ?.FirstOrDefault();
        }

        public IEnumerable<T> List(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes.Aggregate(
                this.dbContext.Set<T>()
                    .AsQueryable(),
                (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings.Aggregate(
                queryableResultWithIncludes,
                (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return secondaryResult.Where(spec.Criteria)
                .AsEnumerable();
        }

        public IEnumerable<T> ListAll()
        {
            return this.dbContext.Set<T>()
                .AsEnumerable();
        }

        public Task<List<T>> ListAllAsync()
        {
            return this.dbContext?.Set<T>()
                .ToListAsync();
        }

        public Task<List<T>> ListAsync(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes.Aggregate(
                this.dbContext.Set<T>()
                    .AsQueryable(),
                (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings.Aggregate(
                queryableResultWithIncludes,
                (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return secondaryResult.Where(spec.Criteria)
                .ToListAsync();
        }

        public async Task<T> Single(ISpecification<T> spec)
        {
            var result = await this.ListAsync(spec)
                                   .ConfigureAwait(false);
            return result.FirstOrDefault();
        }

        public void Update(T entity)
        {
            // this.ApplyChanges(entity);
            try
            {
                this.dbContext.Entry(entity)
                    .State = EntityState.Modified;
                this.dbContext.SaveChanges();
                this.dbContext.Entry(entity)
                    .State = EntityState.Detached;
            }
            catch (Exception e)
            {
                this.dbContext.Entry(entity)
                    .State = EntityState.Detached;

                // this._dbContext.ChangeTracker.AcceptAllChanges();
                throw new Exception(e.Message, e);
            }
        }

        public async Task UpdateAsync(T entity)
        {
            this.dbContext.Entry(entity)
                .State = EntityState.Modified;
            await this.dbContext.SaveChangesAsync()
                .ConfigureAwait(false);
            this.dbContext.Entry(entity)
                .State = EntityState.Detached;
        }

        private EntityState ConvertState(EObjectState state)
        {
            switch (state)
            {
                case EObjectState.Added:
                    return EntityState.Added;
                case EObjectState.Modified:
                    return EntityState.Modified;
                case EObjectState.Deleted:
                    return EntityState.Deleted;
                default:
                    return EntityState.Unchanged;
            }
        }
    }
}
