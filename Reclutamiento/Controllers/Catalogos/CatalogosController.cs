using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ho1a.applicationCore.Utilerias;
using ho1a.reclutamiento.enums.Candidatos;
using ho1a.reclutamiento.services.ViewModels.Catalogos;
using ho1a.reclutamiento.services.Services.Interfaces;
using ho1a.reclutamiento.services.Specifications;
using ho1a.reclutamiento.services.ViewModels.Candidato;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Reclutamiento.Controllers.Catalogos
{
    [Route("api/[controller]")]
    [EnableCors("miRHPolicy")]
    [Authorize]
    public class CatalogosController : ControllerBase
    {
        private readonly ICandidatoService candidatoService;
        private readonly ICatalogService catalogService;
        private readonly IMapper mapper;

        public CatalogosController(IMapper mapper, ICatalogService catalogService, ICandidatoService candidatoService)
        {
            this.mapper = mapper;
            this.catalogService = catalogService;
            this.candidatoService = candidatoService;
        }

        [HttpGet]
        public async Task<ActionResult<CatalogoViewModel>> Get()
        {
            try
            {
                var result = new CatalogoViewModel
                {
                    LocalidadesSucursa = await this.catalogService.GetSelectListLocalidadesAsync()
                                                   .ConfigureAwait(false),
                    PuestoSolicitado = await this.catalogService.GetSelectListPuestos()
                                                   .ConfigureAwait(false),
                    EstadosCivil = await this.catalogService.GetSelectListEstadosCivilAsync()
                                                   .ConfigureAwait(false),
                    UltimosSalarios = await this.catalogService.GetSelectListUltimoSalarioAsync()
                                                   .ConfigureAwait(false),
                    ReferenciasVacante = await this.catalogService.GetSelectListReferenciaVancanteAsync()
                                                   .ConfigureAwait(false)
                };

                return result;
            }
            catch (Exception e)
            {
                this.BadRequest(e.Message);
            }

            return null;
        }

        [Route("Catalogo")]
        [HttpGet]
        public async Task<ActionResult<List<CandidatoBusquedaViewModel>>> GetCatalogoCandidato()
        {
            try
            {
                var candidato = await this.candidatoService.GetAsync(new CandidatoSpecification())
                                          .ConfigureAwait(false);

                var result = this.mapper.Map<List<CandidatoBusquedaViewModel>>(candidato);
                return result;
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [Route("AdministracionExpediente")]
        [HttpGet]
        public IEnumerable<SelectListItem> GetCatalogAdministracionExpediente()
        {
            try
            {
                var tipoContrataciones = Enum.GetValues(typeof(ETipoContratacion))
                    .Cast<ETipoContratacion>();

                var list = new List<SelectListItem>
                               {
                                   new SelectListItem
                                       {
                                           Value = "0",
                                           Text = "SELECCIONE TIPO DE CONTRATACIÓN"
                                       }
                               };
                list.AddRange(
                    tipoContrataciones.Select(
                        contratacion => new SelectListItem
                        {
                            Value = ((int)contratacion).ToString(),
                            Text = contratacion.GetDescription()
                        }));

                return list;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}