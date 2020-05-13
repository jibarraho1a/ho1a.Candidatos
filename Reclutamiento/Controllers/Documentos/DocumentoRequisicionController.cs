using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ho1a.applicationCore.Data.Interfaces;
using ho1a.reclutamiento.models.Candidatos;
using ho1a.reclutamiento.models.Configuracion;
using ho1a.reclutamiento.models.Plazas;
using ho1a.reclutamiento.services.Data.Interfaces;
using ho1a.reclutamiento.services.Services.Interfaces;
using ho1a.reclutamiento.services.Specifications;
using ho1a.reclutamiento.services.ViewModels.Plazas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Reclutamiento.Controllers.Documentos
{
    [Route("api/[controller]")]
    [Authorize]
    [EnableCors("miRHPolicy")]
    public class DocumentoRequisicionController : ControllerBase
    {
        private readonly ICandidatoService candidatoService;
        private readonly IHostingEnvironment environment;
        private readonly IAsyncRepository<ExpedienteArchivo> expedienteArchivoRepository;
        private readonly IAsyncRepository<FileUpload> fileUploadRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly IAsyncRepository<RequisicionArchivo> requisicionArchivoRepository;
        private readonly IAsyncRepository<RequisicionDetalle> requisicionDetalleRepository;
        private readonly IGlobalConfiguration<Configuracion> configuracion;
        private readonly IUploadFileService uploadFileService;

        public DocumentoRequisicionController(
            IMapper mapper,
            IHostingEnvironment environment,
            IHttpContextAccessor httpContextAccessor,
            ICandidatoService candidatoService,
            IAsyncRepository<FileUpload> fileUploadRepository,
            IAsyncRepository<ExpedienteArchivo> expedienteArchivoRepository,
            IAsyncRepository<RequisicionArchivo> requisicionArchivoRepository,
            IAsyncRepository<RequisicionDetalle> requisicionDetalleRepository,
            IGlobalConfiguration<Configuracion> configuracion,
            IUploadFileService uploadFileService)
        {
            this.mapper = mapper;
            this.environment = environment;
            this.httpContextAccessor = httpContextAccessor;
            this.candidatoService = candidatoService;
            this.fileUploadRepository = fileUploadRepository;
            this.expedienteArchivoRepository = expedienteArchivoRepository;
            this.requisicionArchivoRepository = requisicionArchivoRepository;
            this.requisicionDetalleRepository = requisicionDetalleRepository;
            this.configuracion = configuracion;
            this.uploadFileService = uploadFileService;
        }

        [HttpDelete("{idRequisicion}/{idFile}")]
        public async Task<ActionResult<bool>> CandidatoDelete(int idRequisicion, int idFile)
        {
            try
            {
                var requisicion = await this.requisicionDetalleRepository.Single(
                    new RequisicionDetalleSpecification(idRequisicion))
                                            .ConfigureAwait(false);

                RequisicionArchivo requisicionArchivo = null;

                if (requisicion.Propuestas != null)
                {
                    foreach (var requisicionPropuesta in requisicion.Propuestas)
                    {
                        requisicionArchivo =
                            requisicionPropuesta.PropuestaArchivos.FirstOrDefault(
                                p => p?.File != null && p.File.Id == idFile);

                        if (requisicionArchivo != null)
                        {
                            break;
                        }
                    }
                }

                await this.RemoveFileAsync(idFile);

                if (requisicionArchivo != null)
                {
                    requisicionArchivo.File = null;
                    await this.requisicionArchivoRepository.UpdateAsync(requisicionArchivo)
                        .ConfigureAwait(false);
                    return this.Ok();
                }

                return this.NotFound();
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [HttpGet("{idRequisicion}/{idFile}")]
        public async Task<ActionResult> CandidatoGet(int idFile)
        {
            try
            {
                var uploadRepository = this.fileUploadRepository;
                if (uploadRepository != null)
                {
                    var documento = await uploadRepository.GetByIdAsync(idFile)
                                              .ConfigureAwait(false);

                    var path = this.configuracion?.Configuration<string>("UploadsPath");

                    path = Path.Combine(path, documento.FileName);
                    var bytes = System.IO.File.ReadAllBytes(path);
                    return this.File(bytes, this.GetContentType(path), documento.NombreOrigen);
                }
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }

            return this.NotFound();
        }

        [HttpPost("{idRequisicion}/{idExpediente}")]
        public async Task<ActionResult<FileViewModel>> CandidatoPost(
            int idRequisicion,
            int idExpediente,
            IFormFile file)
        {
            try
            {
                var candidato = await this.candidatoService.GetCandidatoIdoneoAsync(idRequisicion)
                                          .ConfigureAwait(false);

                var fileViewModel = await this.GetFileInfo(file)
                                              .ConfigureAwait(false);

                fileViewModel.RequisicionId = idRequisicion;

                var fileUpload = this.mapper.Map<FileUpload>(fileViewModel);

                var result = await this.uploadFileService.AddRequisicionArchivo(idRequisicion, idExpediente, fileUpload)
                                       .ConfigureAwait(false);

                return this.mapper.Map<FileViewModel>(result);
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        private string GetContentType(string path)
        {
            var types = this.GetMimeTypes();
            var ext = Path.GetExtension(path)
                ?.ToLowerInvariant();
            return types?[ext];
        }

        private async Task<FileViewModel> GetFileInfo(IFormFile file)
        {
            var request = this.httpContextAccessor.HttpContext.Request;
            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Path = request.Path.ToString()
            };
            var uri = uriBuilder.Uri;

            var stream = file.OpenReadStream();
            var fileName = file.FileName;
            var hostingEnvironment = this.environment;
            FileViewModel info = null;

            if (hostingEnvironment == null)
            {
                return null;
            }

            var path = this.configuracion?.Configuration<string>("UploadsPath");


            var filePath = Path.Combine(path, fileName);
            var ext = Path.GetExtension(filePath);
            var fileNameDestino = $"{DateTime.Now: ddMMyyyHHmmssfffffff}{ext}".Trim();
            filePath = Path.Combine(path, fileNameDestino);

            using (var newFile = new FileStream(filePath, FileMode.Create))
            {
                await stream.CopyToAsync(newFile)
                    .ConfigureAwait(false);
            }

            info = new FileViewModel
            {
                FechaActualizacion = DateTime.Now,
                NombreOrigen = fileName,
                NombreDestino = fileNameDestino,
                Url = uri.ToString()
            };

            return info;
        }

        private async Task RemoveFileAsync(int idFile)
        {
            try
            {
                var path = this.configuracion?.Configuration<string>("UploadsPath");

                var fileInfo = await this.fileUploadRepository.GetByIdAsync(idFile);

                if (fileInfo != null)
                {
                    var toDelete = fileInfo.FileName;
                    var filePath = Path.Combine(path, toDelete);
                    System.IO.File.Delete(filePath);
                }
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            var contentRootPath = this.environment?.ContentRootPath;
            var jsonFile = System.IO.File.ReadAllText(contentRootPath + "/mimes.json");
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile);
            return dict;
        }
    }
}