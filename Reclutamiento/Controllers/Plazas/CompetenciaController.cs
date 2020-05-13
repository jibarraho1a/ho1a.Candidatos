using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ho1a.applicationCore.Data.Interfaces;
using ho1a.reclutamiento.models.Plazas;
using ho1a.reclutamiento.services.Data.Interfaces;
using ho1a.reclutamiento.services.Services.Interfaces;
using ho1a.reclutamiento.services.Specifications;
using ho1a.reclutamiento.services.ViewModels.Plazas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Reclutamiento.Controllers.Plazas
{
    [Route("api/[controller]")]
    [EnableCors("miRHPolicy")]
    [Authorize]
    public class CompetenciaController : BaseController<Entrevista, EntrevistaViewModel, EntrevistaViewModel>
    {
        private readonly IAsyncRepository<Entrevista> entrevistaRepository;
        private readonly IMapper mapper;

        public CompetenciaController(
            IUserResolverService userResolverService,
            IMapper mapper,
            IGeneralService<Entrevista> service,
            IAsyncRepository<Entrevista> entrevistaRepository)
            : base(userResolverService, mapper, service)
        {
            this.mapper = mapper;
            this.entrevistaRepository = entrevistaRepository;
        }

        [HttpPut("{idRequisicion}/{idEntrevista}")]
        public async Task<ActionResult> Put(
            string UserName,
            int idRequisicion,
            int idEntrevista,
            [FromBody] List<CompetenciaViewModel> competencias)
        {
            try
            {
                var resultEEntrevistas = await this.entrevistaRepository.ListAsync(
                    new EntrevistaSpecification(idRequisicion, idEntrevista))
                                                   .ConfigureAwait(false);

                var entrevista = resultEEntrevistas.FirstOrDefault();

                foreach (var competencia in competencias)
                {
                    if (competencia.Id == 0)
                    {
                        var toInsert = this.mapper.Map<Competencia>(competencia);
                        entrevista.Competencias.Add(toInsert);
                    }
                    else
                    {
                        var toEdit = entrevista.Competencias.FirstOrDefault(l => l.Id == competencia.Id);

                        toEdit.Nombre = competencia.Nombre;
                        toEdit.Resultado = competencia.Resultado;
                        toEdit.Descripcion = competencia.Descripcion;
                    }
                }

                var entrevistaToEdit = this.mapper.Map<EntrevistaViewModel>(entrevista);

                await this.Put(UserName, idEntrevista, entrevistaToEdit)
                    .ConfigureAwait(false);

                return this.Ok();
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }
    }
}