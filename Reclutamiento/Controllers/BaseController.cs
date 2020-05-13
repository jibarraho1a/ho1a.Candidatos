using AutoMapper;
using ho1a.applicationCore.Data.Interfaces;
using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.services.Data.Interfaces;
using ho1a.reclutamiento.services.Services.Interfaces;
using ho1a.reclutamiento.services.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reclutamiento.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [EnableCors("miRHPolicy")]
    public abstract class BaseController<TEntity, TViewModel, TListViewModel> : ControllerBase
        where TEntity : BaseEntityId, new() where TViewModel : BaseViewModel, new() where TListViewModel : new()
    {

        private readonly IMapper mapper;
        private readonly IGeneralService<TEntity> service;
        private readonly IUserResolverService userResolverService;

        protected BaseController(
            IUserResolverService userResolverService,
            IMapper mapper,
            IGeneralService<TEntity> service)
        {
            this.userResolverService = userResolverService;
            this.mapper = mapper;
            this.service = service;

            this.Specification = null;
        }

        protected ISpecification<TEntity> Specification { get; set; }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TViewModel>> Delete(int id)
        {
            var result = this.Specification != null
                             ? await this.service.DeleteAsync(this.Specification)
                                   .ConfigureAwait(false)
                             : await this.service.DeleteAsync(id)
                                   .ConfigureAwait(false);

            return result
                       ? (ActionResult<TViewModel>)this.NoContent()
                       : this.BadRequest();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public virtual async Task<ActionResult<TViewModel>> Get(string UserName, int id)
        {
            try
            {
                if (this.Specification != null)
                {
                    return await this.GetBySpecification(this.Specification)
                               .ConfigureAwait(false);
                }

                return await this.GetById(UserName, id)
                           .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                this.BadRequest(e.Message);
            }

            return this.NotFound();
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public virtual async Task<ActionResult<List<TListViewModel>>> Get()
        {
            List<TListViewModel> result;
            try
            {
                if (this.Specification != null)
                {
                    var response = await this.service.GetAsync(this.Specification)
                                       .ConfigureAwait(false);

                    if (response == null)
                    {
                        return this.NotFound();
                    }

                    result = this.mapper?.Map<List<TListViewModel>>(response);
                }
                else
                {
                    var response = await this.service.GetAsync()
                                       .ConfigureAwait(false);

                    if (response == null)
                    {
                        return this.NotFound();
                    }

                    result = this.mapper?.Map<List<TListViewModel>>(response);
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }

            return result;
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public virtual async Task<ActionResult<TViewModel>> Post(string UserName, TViewModel entity)
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return this.BadRequest(this.ModelState);
                }

                var toInsert = this.mapper.Map<TEntity>(entity);

                toInsert.CreatedBy = UserName;
                toInsert.Created = DateTime.Now;

                var response = await this.service.AddAsync(toInsert)
                                   .ConfigureAwait(false);

                var result = this.mapper?.Map<TViewModel>(response);
                return result;
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public virtual async Task<ActionResult<TViewModel>> Put(string UserName, int id, [FromBody] TViewModel entity)
        {
            try
            {
                var modelStateDictionary = this.ModelState;
                if (modelStateDictionary?.IsValid == false)
                {
                    return this.BadRequest(this.ModelState);
                }

                var toEdit = this.mapper?.Map<TEntity>(entity);

                toEdit.EditedBy = UserName;
                toEdit.Edited = DateTime.Now;

                this.service.ByIdSpecification = this.Specification;
                var result = await service.UpdateAsync(id, toEdit)
                                 .ConfigureAwait(true);

                var itemToReturn = this.mapper.Map<TViewModel>(result);

                return itemToReturn ?? (ActionResult<TViewModel>)this.NoContent();
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        private async Task<ActionResult<TViewModel>> GetById(string UserName, int id)
        {
            try
            {
                TEntity response;

                if (id != 0)
                {
                    response = await this.service.GetAsync(id)
                                   .ConfigureAwait(false);

                    if (response == null)
                    {
                        return this.NotFound();
                    }
                }
                else
                {
                    var toInsert = new TViewModel();
                    return await this.Post(UserName, toInsert)
                               .ConfigureAwait(false);
                }

                var result = this.mapper?.Map<TViewModel>(response);
                return result;
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        private async Task<ActionResult<TViewModel>> GetBySpecification(ISpecification<TEntity> specification)
        {
            var result = default(TViewModel);

            try
            {
                if (specification != null)
                {
                    var response = await this.service.GetAsync(specification)
                                       .ConfigureAwait(false);

                    if (response == null)
                    {
                        return this.NotFound();
                    }

                    if (!response.Any())
                    {
                        return this.NotFound();
                    }

                    var item = response.FirstOrDefault();
                    result = this.mapper?.Map<TViewModel>(item);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return result;
        }
    }
}