using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ho1a.applicationCore.Data.Interfaces;
using ho1a.reclutamiento.models.Candidatos;
using ho1a.reclutamiento.models.Configuracion;
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
    [EnableCors("miRHPolicy")]
    [Authorize]
    public class DocumentoExpedienteController : ControllerBase
    {
        private readonly ICandidatoService candidatoService;
        private readonly IHostingEnvironment environment;
        private readonly IAsyncRepository<ExpedienteArchivo> expedienteArchivoRepository;
        private readonly IGlobalConfiguration<Configuracion> configuracion;
        private readonly IAsyncRepository<FileUpload> fileUploadRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly IUploadFileService uploadFileService;
        
        public DocumentoExpedienteController(
            IMapper mapper,
            IHostingEnvironment environment,
            IHttpContextAccessor httpContextAccessor,
            ICandidatoService candidatoService,
            IAsyncRepository<FileUpload> fileUploadRepository,
            IAsyncRepository<ExpedienteArchivo> expedienteArchivoRepository,
            IGlobalConfiguration<Configuracion> configuracion,
            IUploadFileService uploadFileService)
        {
            this.mapper = mapper;
            this.environment = environment;
            this.httpContextAccessor = httpContextAccessor;
            this.candidatoService = candidatoService;
            this.fileUploadRepository = fileUploadRepository;
            this.expedienteArchivoRepository = expedienteArchivoRepository;
            this.configuracion = configuracion;
            this.uploadFileService = uploadFileService;
        }

        [HttpDelete("{idCandidato}/{idFile}")]
        public async Task<ActionResult<bool>> CandidatoDelete(int idCandidato, int idFile)
        {
            try
            {
                var candidato = this.candidatoService.Single(new CandidatoSpecification(idCandidato));

                var expedienteArchivo =
                    candidato?.CandidatoDetalle?.CandidatoExpediente?.ExpedientesArchivos?.FirstOrDefault(
                        e => e != null && e.File?.Id == idFile);

                await this.RemoveFileAsync(idFile);

                if (expedienteArchivo != null)
                {
                    expedienteArchivo.File = null;

                    await this.expedienteArchivoRepository.UpdateAsync(expedienteArchivo)
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

        [HttpGet("{idCandidato}/{idFile}")]
        public async Task<ActionResult> CandidatoGet(int idCandidato, int idFile)
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

        [HttpPost("{idCandidato}/{idExpediente}")]
        public async Task<ActionResult<FileViewModel>> CandidatoPost(int idCandidato, int idExpediente, IFormFile file)
        {
            try
            {
                // var candidato = this.candidatoService.Single(new CandidatoSpecification(idRequisicion));
                // var candidato = await this.candidatoService.GetCandidatoIdoneoAsync(idRequisicion)
                // .ConfigureAwait(false);
                var fileViewModel = await this.GetFileInfo(file)
                                              .ConfigureAwait(false);

                // fileViewModel.RequisicionId = idRequisicion;
                var fileUpload = this.mapper.Map<FileUpload>(fileViewModel);

                var result = await this.uploadFileService.AddExpedienteArchivo(0, idCandidato, idExpediente, fileUpload)
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