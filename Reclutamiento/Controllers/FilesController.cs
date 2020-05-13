using AutoMapper;
using ho1a.applicationCore.Data.Interfaces;
using ho1a.reclutamiento.models.Candidatos;
using ho1a.reclutamiento.services.Data.Interfaces;
using ho1a.reclutamiento.services.Services.Interfaces;
using ho1a.reclutamiento.services.ViewModels.Plazas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Reclutamiento.Controllers
{
    [Authorize]
    [EnableCors("miRHPolicy")]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly ICandidatoService candidatoService;
        private readonly IHostingEnvironment environment;
        private readonly IAsyncRepository<FileUpload> fileUploadRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;
        private readonly IUploadFileService uploadFileService;

        public FilesController(
            IMapper mapper,
            IHostingEnvironment environment,
            IHttpContextAccessor httpContextAccessor,
            IAsyncRepository<FileUpload> fileUploadRepository,
            ICandidatoService candidatoService,
            IUploadFileService uploadFileService)
        {
            this.mapper = mapper;
            this.environment = environment;
            this.httpContextAccessor = httpContextAccessor;
            this.fileUploadRepository = fileUploadRepository;
            this.candidatoService = candidatoService;
            this.uploadFileService = uploadFileService;
        }

        [Route("Requisicion/{idFile}")]
        [HttpGet("{idFile}")]
        public async Task<ActionResult> RequisicionGet(int idFile)
        {
            try
            {
                var uploadRepository = this.fileUploadRepository;
                if (uploadRepository != null)
                {
                    var documento = await uploadRepository.GetByIdAsync(idFile)
                                              .ConfigureAwait(false);

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", documento.FileName);
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

        [Route("Requisicion/{idRequisicion}/{idExpediente}")]
        [HttpPost("{idRequisicion}/{idExpediente}")]
        public async Task<ActionResult<FileViewModel>> RequisicionPost(
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

            var filePath = Path.Combine(hostingEnvironment.ContentRootPath, @"Uploads", fileName);
            var ext = Path.GetExtension(filePath);
            var fileNameDestino = $"{DateTime.Now: ddMMyyyHHmmssfffffff}{ext}".Trim();
            filePath = Path.Combine(hostingEnvironment.ContentRootPath, @"Uploads", fileNameDestino);

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

        private Dictionary<string, string> GetMimeTypes()
        {
            var contentRootPath = this.environment?.ContentRootPath;
            var jsonFile = System.IO.File.ReadAllText(contentRootPath + "/mimes.json");
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFile);
            return dict;
        }
    }
}
