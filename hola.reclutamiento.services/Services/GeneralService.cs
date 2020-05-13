using ho1a.applicationCore.Data.Interfaces;
using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.services.Data.Interfaces;
using ho1a.reclutamiento.services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services
{
    public class GeneralService<T> : IGeneralService<T>
        where T : BaseEntityId, new()
    {

        private readonly IAsyncRepository<T> asyncRepository;
        private readonly IRepository<T> repository;

        public GeneralService(IAsyncRepository<T> asyncRepository, IRepository<T> repository)
        {
            this.asyncRepository = asyncRepository;
            this.repository = repository;
        }

        public ISpecification<T> AllSpecification { get; set; }

        public ISpecification<T> ByIdSpecification { get; set; }

        public async Task<T> AddAsync(T item)
        {
            T result;
            try
            {
                result = await this.asyncRepository.AddAsync(item)
                                   .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }

            return result;
        }

        public async Task<bool> DeleteAsync(ISpecification<T> specification)
        {
            try
            {
                var result = await this.asyncRepository.ListAsync(this.ByIdSpecification)
                                       .ConfigureAwait(false);

                if (result.Any())
                {
                    if (result.Count == 1)
                    {
                        var toEdit = result.FirstOrDefault();
                        if (toEdit != null)
                        {
                            toEdit.Active = false;
                            await this.asyncRepository.UpdateAsync(toEdit)
                                .ConfigureAwait(false);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var result = await this.asyncRepository.GetByIdAsync(id)
                                       .ConfigureAwait(false);

                if (result != null && result.Active)
                {
                    result.Active = false;
                    await this.asyncRepository.UpdateAsync(result)
                        .ConfigureAwait(false);
                }
                else
                {
                    throw new Exception("Elemento no encontrado.");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return true;
        }

        public async Task<List<T>> GetAsync()
        {
            List<T> result;
            try
            {
                result = await this.asyncRepository.ListAllAsync()
                                   .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return result;
        }

        public async Task<List<T>> GetAsync(ISpecification<T> specification)
        {
            List<T> result;
            try
            {
                var response = await this.asyncRepository.ListAsync(specification)
                                         .ConfigureAwait(false);

                result = response;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return result;
        }

        public async Task<T> GetAsync(int id)
        {
            T result;
            try
            {
                result = await this.asyncRepository.GetByIdAsync(id)
                                   .ConfigureAwait(false);

                var isActive = result != null && result.Active;

                if (!isActive)
                {
                    throw new Exception("Elemento no encontrado");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return result;
        }

        public T Single(ISpecification<T> specification)
        {
            return this.GetAsync(specification)
                .Result?.FirstOrDefault();
        }

        public async Task<T> UpdateAsync(int id, T item)
        {
            T toEdit;
            try
            {
                if (this.ByIdSpecification == null)
                {
                    toEdit = this.repository.GetById(id);
                }
                else
                {
                    var result = this.repository.List(this.ByIdSpecification);
                    toEdit = result.FirstOrDefault();
                }

                item.Created = toEdit.Created;
                item.CreatedBy = toEdit.CreatedBy;

                item.Edited = DateTime.Now;
                item.EditedBy = toEdit.EditedBy;

                this.repository.ApplyCurrentValues(toEdit, item);
                this.repository.Update(toEdit);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return toEdit;
        }
    }
}