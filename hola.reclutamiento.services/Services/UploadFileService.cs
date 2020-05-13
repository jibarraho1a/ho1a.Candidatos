using ho1a.applicationCore.Data.Interfaces;
using ho1a.reclutamiento.models.Candidatos;
using ho1a.reclutamiento.models.Configuracion;
using ho1a.reclutamiento.models.Plazas;
using ho1a.reclutamiento.services.Data.Interfaces;
using ho1a.reclutamiento.services.Services.Interfaces;
using ho1a.reclutamiento.services.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services
{
    public class UploadFileService : GeneralService<FileUpload>, IUploadFileService
    {
        private readonly IAsyncRepository<CandidatoDetalle> candidatoDetalleRepository;
        private readonly IAsyncRepository<CandidatoExpediente> candidatoExpedienteRepository;
        private readonly IAsyncRepository<Candidato> candidatoRepository;
        private readonly IGlobalConfiguration<Configuracion> configuracionGlobal;
        private readonly IAsyncRepository<ExpedienteArchivo> expedienteArchivoRepository;
        private readonly IAsyncRepository<FileUpload> repository;
        private readonly IAsyncRepository<RequisicionArchivo> requisicionArchivoRepository;
        private readonly IAsyncRepository<RequisicionDetalle> requisicionRepository;

        public UploadFileService(
            IGlobalConfiguration<Configuracion> configuracionGlobal,
            IAsyncRepository<FileUpload> asyncRepository,
            IAsyncRepository<CandidatoDetalle> candidatoDetalleRepository,
            IAsyncRepository<CandidatoExpediente> candidatoExpedienteRepository,
            IAsyncRepository<ExpedienteArchivo> expedienteArchivoRepository,
            IAsyncRepository<RequisicionArchivo> requisicionArchivoRepository,
            IRepository<FileUpload> repository,
            IAsyncRepository<Candidato> candidatoRepository,
            IAsyncRepository<RequisicionDetalle> requisicionRepository)
            : base(asyncRepository, repository)
        {
            this.configuracionGlobal = configuracionGlobal;
            this.repository = asyncRepository;
            this.candidatoDetalleRepository = candidatoDetalleRepository;
            this.candidatoExpedienteRepository = candidatoExpedienteRepository;
            this.expedienteArchivoRepository = expedienteArchivoRepository;
            this.requisicionArchivoRepository = requisicionArchivoRepository;
            this.candidatoRepository = candidatoRepository;
            this.requisicionRepository = requisicionRepository;
        }

        public async Task<FileUpload> AddExpedienteArchivo(
            int idRequisicion,
            int idCandidato,
            int idExpediente,
            FileUpload fileUpload,
            int? idExpedienteArchivo = null)
        {
            fileUpload = await this.repository.AddAsync(fileUpload)
                                   .ConfigureAwait(false);

            var candidato = await this.candidatoRepository.Single(new CandidatoSpecification(idCandidato))
                                      .ConfigureAwait(false);

            var detalle = candidato.CandidatoDetalle;

            if (detalle.Id == 0)
            {
                detalle.CandidatoId = idCandidato;
                await this.candidatoDetalleRepository.AddAsync(detalle)
                    .ConfigureAwait(false);

                candidato = await this.candidatoRepository.Single(new CandidatoSpecification(idCandidato))
                                      .ConfigureAwait(false);
            }

            if (candidato.CandidatoDetalle.CandidatoExpediente.Id == 0)
            {
                candidato.CandidatoDetalle.CandidatoExpediente.Id = candidato.CandidatoDetalle.Id;
                await this.candidatoExpedienteRepository.AddAsync(candidato.CandidatoDetalle.CandidatoExpediente)
                    .ConfigureAwait(false);
            }

            var candidatoExpediente = candidato.CandidatoDetalle.CandidatoExpediente
                                      ?? new CandidatoExpediente
                                      {
                                          CandidatoDetalleId = candidato.CandidatoDetalle.Id,
                                          ExpedientesArchivos = new List<ExpedienteArchivo>()
                                      };

            var expedienteArchivo = idExpedienteArchivo == null
                                        ? candidatoExpediente?.ExpedientesArchivos?.FirstOrDefault(
                                            e => e.ExpedienteId == idExpediente)
                                        : candidatoExpediente?.ExpedientesArchivos?.FirstOrDefault(
                                            e =>
                                            e.ExpedienteId == idExpediente
                                            && e.Id == idExpedienteArchivo);
            if (expedienteArchivo != null)
            {
                expedienteArchivo.File = fileUpload;

                await this.expedienteArchivoRepository.UpdateAsync(expedienteArchivo)
                    .ConfigureAwait(false);
            }
            else
            {
                if (candidatoExpediente.ExpedientesArchivos == null)
                {
                    candidatoExpediente.ExpedientesArchivos = new List<ExpedienteArchivo>();
                }

                expedienteArchivo = new ExpedienteArchivo { File = fileUpload, ExpedienteId = idExpediente };

                candidatoExpediente.ExpedientesArchivos.Add(expedienteArchivo);
            }

            if (candidatoExpediente.Id == 0)
            {
                candidatoExpediente = await this.candidatoExpedienteRepository.AddAsync(candidatoExpediente)
                                                .ConfigureAwait(false);
            }

            this.AddNomina(candidatoExpediente, expedienteArchivo.ExpedienteId.Value);

            await this.candidatoExpedienteRepository.UpdateAsync(candidatoExpediente)
                .ConfigureAwait(false);

            return fileUpload;
        }

        public async Task<FileUpload> AddRequisicionArchivo(int idRequisicion, int idExpediente, FileUpload fileUpload)
        {
            fileUpload = await this.repository.AddAsync(fileUpload)
                                   .ConfigureAwait(false);

            var requisicionDetalle = await this.requisicionRepository.Single(
                new RequisicionDetalleSpecification(idRequisicion))
                                               .ConfigureAwait(false);

            var requisicionArchivo =
                requisicionDetalle.Propuesta.PropuestaArchivos.FirstOrDefault(e => e.ExpedienteId == idExpediente);

            if (requisicionArchivo != null)
            {
                requisicionArchivo.File = fileUpload;

                await this.requisicionArchivoRepository.UpdateAsync(requisicionArchivo)
                    .ConfigureAwait(false);
            }
            else
            {
                requisicionArchivo = new RequisicionArchivo
                {
                    File = fileUpload,
                    ExpedienteId = idExpediente
                };

                requisicionDetalle.Propuesta.PropuestaArchivos.Add(requisicionArchivo);

                await this.requisicionRepository.UpdateAsync(requisicionDetalle)
                    .ConfigureAwait(false);
            }

            return fileUpload;
        }

        public async Task<bool> DeleteExpedienteArchivo(int idRequisicion, int idCandidato, int idExpediente)
        {
            var resultCandidatos = await this.candidatoExpedienteRepository.ListAsync(
                new CandidatoExpedienteSpecification(idCandidato))
                                             .ConfigureAwait(false);
            var candidato = resultCandidatos.FirstOrDefault();

            if (candidato == null)
            {
                return false;
            }

            var expedienteArchivo = candidato.ExpedientesArchivos.FirstOrDefault(e => e.Expediente.Id == idExpediente);
            if (expedienteArchivo != null)
            {
                await this.expedienteArchivoRepository.DeleteAsync(expedienteArchivo)
                    .ConfigureAwait(false);
            }

            if (candidato.Id != 0)
            {
                await this.candidatoExpedienteRepository.UpdateAsync(candidato)
                    .ConfigureAwait(false);
            }

            return true;
        }

        private async void AddNomina(CandidatoExpediente candidato, int idExpediente)
        {
            var idExpedienteNomina = this.configuracionGlobal?.Configuration<int>("ItemDocumentoIdNomina");

            if (idExpedienteNomina != idExpediente)
            {
                return;
            }

            var nominasDisponibles =
                candidato.ExpedientesArchivos.Count(e => e.File == null && e.ExpedienteId == idExpediente);

            if (nominasDisponibles > 0)
            {
                return;
            }

            var expedienteArchivo = new ExpedienteArchivo
            {
                File = null,

                // CandidatoExpedienteId = candidato.CandidatoDetalle.CandidatoExpediente.Id,
                ExpedienteId = idExpediente
            };
            candidato.ExpedientesArchivos?.Add(expedienteArchivo);
        }
    }
}