using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ho1a.reclutamiento.models.Plazas;
using ho1a.reclutamiento.services.Data.Interfaces;
using ho1a.reclutamiento.services.Services.Interfaces;
using ho1a.reclutamiento.services.Specifications;
using ho1a.reclutamiento.services.ViewModels.Candidato;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Reclutamiento.Controllers.Documentos
{
    using ho1a.applicationCore.Data.Interfaces;
    using ho1a.reclutamiento.models.Candidatos;

    [Route("api/[controller]")]
    [EnableCors("miRHPolicy")]
    [Authorize]
    public class DocumentosController :
        BaseController<CandidatoExpediente, ExpedienteFileViewModel, ExpedienteFileViewModel>
    {
        private readonly IExpedienteCandidatoService expedienteCandidatoService;
        private readonly IMapper mapper;
        private readonly IAsyncRepository<Candidato> candidatoRepository;
        private readonly IAsyncRepository<Requisicion> requisicionRepository;
        private readonly IUserResolverService userResolverService;

        public DocumentosController(
            IUserResolverService userResolverService,
            IMapper mapper,
            IGeneralService<CandidatoExpediente> service,
            IAsyncRepository<Candidato> candidatoRepository,
            IAsyncRepository<Requisicion> requisicionRepository,
            IExpedienteCandidatoService expedienteCandidatoService)
            : base(userResolverService, mapper, service)
        {
            this.userResolverService = userResolverService;
            this.mapper = mapper;
            this.candidatoRepository = candidatoRepository;
            this.requisicionRepository = requisicionRepository;
            this.expedienteCandidatoService = expedienteCandidatoService;
        }

        [Route("CandidatoDocumento/{idCandidato}")]
        [HttpGet("{idRequisicion}")]
        public async Task<ActionResult<List<ExpedienteFileViewModel>>> CandidatoGet(int idCandidato)
        {
            try
            {
                var result = await this.expedienteCandidatoService.GetDocumentosCandidato(idCandidato)
                                       .ConfigureAwait(false);

                var expedientes = new List<ExpedienteFileViewModel>();

                foreach (var expedientesFiles in result.Where(r => r?.ExpedienteArchivos?.Count > 1)
                    .Select(
                        expediente =>
                        this.mapper?.Map<List<ExpedienteFileViewModel>>(expediente?.ExpedienteArchivos?.ToList())))
                {
                    expedientes.AddRange(expedientesFiles);
                }

                foreach (var expedientesFiles in result.Where(r => r?.ExpedienteArchivos?.Count == 1)
                    .Select(
                        expediente =>
                        this.mapper?.Map<List<ExpedienteFileViewModel>>(expediente?.ExpedienteArchivos?.ToList())))
                {
                    expedientes.AddRange(expedientesFiles);
                }

                expedientes.AddRange(
                    result.Where(r => r?.ExpedienteArchivos?.Count == 0 || r?.ExpedienteArchivos == null)
                        .Select(expedientesFiles => this.mapper?.Map<ExpedienteFileViewModel>(expedientesFiles)));

                foreach (var expediente in expedientes)
                {
                    expediente.Path = expediente.Path?.Replace("{idCandidato}", idCandidato.ToString());
                }

                return expedientes.OrderBy(e => e.Name)
                    .ThenBy(e => e.Loaded)
                    .ToList();
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [Route("CV/{idCandidato}")]
        [HttpGet("{idRequisicion}")]
        public async Task<ActionResult<List<ExpedienteFileViewModel>>> CV(int idCandidato)
        {
            try
            {
                var result = await this.expedienteCandidatoService.GetDocumentosCandidato(idCandidato)
                                       .ConfigureAwait(false);

                var expedientes = new List<ExpedienteFileViewModel>();

                foreach (var expedientesFiles in result.Where(r => r.ExpedienteArchivos != null && r.ExpedienteArchivos.Count == 1 && r.ExpedienteArchivos.Any(e => e.ExpedienteId == 27))
                    .Select(
                        expediente =>
                        this.mapper?.Map<List<ExpedienteFileViewModel>>(expediente?.ExpedienteArchivos?.ToList())))
                {
                    expedientes.AddRange(expedientesFiles);
                }

                expedientes.AddRange(
                    result.Where(r => r?.ExpedienteArchivos?.Count == 0 || r?.ExpedienteArchivos == null)
                        .Select(expedientesFiles => this.mapper?.Map<ExpedienteFileViewModel>(expedientesFiles)));

                foreach (var expediente in expedientes)
                {
                    expediente.Path = expediente.Path?.Replace("{idCandidato}", idCandidato.ToString());
                }

                return expedientes.OrderBy(e => e.Name)
                    .ThenBy(e => e.Loaded)
                    .ToList();
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [Route("Expediente/{idRequisicion}/{idCandidato}")]
        [HttpGet("{idRequisicion}/{idCandidato}")]
        public async Task<ActionResult<List<ExpedienteFileViewModel>>> ExpedienteGet(int idRequisicion, int idCandidato)
        {
            try
            {
                var result = await this.expedienteCandidatoService.GetExpedienteCandidato(idCandidato)
                                 .ConfigureAwait(false);

                var expedienteAdministrador = await this.expedienteCandidatoService.GetExpedienteAdministrador(idCandidato)
                                                  .ConfigureAwait(false);

                var candidato = await this.candidatoRepository.Single(new CandidatoSpecification(idCandidato));
                var candidatoExpediente = candidato?.CandidatoDetalle?.CandidatoExpediente;
                var colaboradorCarga = candidatoExpediente?.FechaNotificacion != null;
                var expedientesValidacion = candidatoExpediente?.FechaValidaExpediente != null;
                var requiereAtencion = candidatoExpediente?.CorregirExpediente ?? false;
                var readOnly = candidatoExpediente?.FechaNotificacion == null
                               && candidatoExpediente?.FechaValidaExpediente == null
                               && candidatoExpediente?.CorregirExpediente == null;

                var expedientesAdministrador = this.mapper.Map<List<ExpedienteFileViewModel>>(expedienteAdministrador);

                foreach (var expediente in expedientesAdministrador)
                {
                    if (readOnly)
                    {
                        expediente.ReadOnly = false;
                    }
                    else
                    {
                        if (requiereAtencion)
                        {
                            expediente.ReadOnly = true;
                        }
                        else
                        {
                            expediente.ReadOnly = expedientesValidacion && colaboradorCarga;
                        }
                    }

                    expediente.Path = expediente.Path?.Replace("{idCandidato}", idCandidato.ToString());
                }

                var expedientes = this.mapper.Map<List<ExpedienteFileViewModel>>(result);

                foreach (var expediente in expedientes)
                {
                    if (readOnly)
                    {
                        expediente.ReadOnly = readOnly;
                    }
                    else
                    {
                        if (requiereAtencion)
                        {
                            expediente.ReadOnly = true;
                        }
                        else
                        {
                            expediente.ReadOnly = expedientesValidacion && colaboradorCarga;
                        }
                    }

                    expediente.Path = expediente.Path?.Replace("{idCandidato}", idCandidato.ToString());
                }

                expedientesAdministrador.AddRange(expedientes);
                return expedientesAdministrador;
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [Route("Expediente")]
        [HttpGet]
        public async Task<ActionResult<List<ExpedienteFileViewModel>>> ExpedienteGet(int? candidatoId)
        {
            try
            {
                if (candidatoId != null)
                {
                    var result = await this.expedienteCandidatoService.GetExpedienteCandidato(candidatoId.Value)
                                           .ConfigureAwait(false);

                    var candidatoExpediente = result.FirstOrDefault()
                        ?.ExpedienteArchivos?.FirstOrDefault()
                        ?.CandidatoExpediente;

                    var sendToReview = candidatoExpediente?.FechaNotificacion != null;
                    var needAttention = candidatoExpediente?.CorregirExpediente ?? false;

                    var expedientes = this.mapper.Map<List<ExpedienteFileViewModel>>(result);

                    foreach (var expediente in expedientes)
                    {
                        expediente.ReadOnly = sendToReview && !needAttention;
                        expediente.Path = expediente.Path?.Replace("{idCandidato}", candidatoId.ToString());
                    }

                    return expedientes;
                }

                return this.NoContent();
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [Route("RequisicionPropuesta/{idRequisicion}/{idCandidato}")]
        [HttpGet("{idRequisicion}")]
        public async Task<ActionResult<List<ExpedienteFileViewModel>>> RequisicionGet(
            int idRequisicion,
            int idCandidato)
        {
            try
            {
                var result = await this.expedienteCandidatoService.GetDocumentosPropuesta(idRequisicion, idCandidato)
                                       .ConfigureAwait(false);

                if (result == null)
                {
                    return this.NoContent();
                }

                var expedientes = this.mapper.Map<List<ExpedienteFileViewModel>>(result);

                foreach (var expediente in expedientes)
                {
                    expediente.Path = expediente.Path?.Replace("{idRequisicion}", idRequisicion.ToString());
                }

                var documentosCandidatos = await this.CandidatoGet(idCandidato)
                                                     .ConfigureAwait(false);

                var documentos = new List<ExpedienteFileViewModel>();

                if (documentosCandidatos.Value == null)
                {
                    return documentos;
                }

                foreach (var documentosCandidato in documentosCandidatos.Value)
                {
                    documentosCandidato.ReadOnly = true;
                }

                var requisicion = await this.requisicionRepository.Single(new RequisicionSpecification(idRequisicion))
                                            .ConfigureAwait(false);

                if (requisicion.RequisicionDetalle.Propuesta != null)
                {
                    if (requisicion.RequisicionDetalle.Propuesta.FechaEnvioPropuesta != null)
                    {
                        foreach (var expediente in expedientes)
                        {
                            expediente.ReadOnly = true;
                        }
                    }
                }

                documentos.AddRange(expedientes);
                documentos.AddRange(documentosCandidatos.Value.Where(d => d.Loaded));

                return documentos;
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }
    }
}
