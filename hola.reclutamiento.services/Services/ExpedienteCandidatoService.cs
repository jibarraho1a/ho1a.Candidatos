using ho1a.applicationCore.Data.Interfaces;
using ho1a.reclutamiento.enums.Catalogos;
using ho1a.reclutamiento.models.Candidatos;
using ho1a.reclutamiento.models.Catalogos;
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
    public class ExpedienteCandidatoService : GeneralService<CandidatoExpediente>, IExpedienteCandidatoService
    {
        private readonly IAsyncRepository<CandidatoExpediente> asyncRepository;
        private readonly IAsyncRepository<ExpedienteArchivo> expedienteArchivoRepository;
        private readonly IAsyncRepository<Expediente> expedienteRepository;
        private readonly IAsyncRepository<RequisicionDetalle> requisicionDetalleRepository;

        public ExpedienteCandidatoService(
            IAsyncRepository<RequisicionDetalle> requisicionDetalleRepository,
            IAsyncRepository<CandidatoExpediente> asyncRepository,
            IRepository<CandidatoExpediente> repository,
            IAsyncRepository<Expediente> expedienteRepository,
            IAsyncRepository<ExpedienteArchivo> expedienteArchivoRepository)
            : base(asyncRepository, repository)
        {
            this.requisicionDetalleRepository = requisicionDetalleRepository;
            this.asyncRepository = asyncRepository;
            this.expedienteRepository = expedienteRepository;
            this.expedienteArchivoRepository = expedienteArchivoRepository;
        }

        public async Task<List<Expediente>> GetDocumentosCandidato(int idCandidato)
        {
            var candidatoExpediente = await this.expedienteRepository.ListAllAsync()
                                          .ConfigureAwait(false);

            foreach (var expediente in candidatoExpediente)
            {
                expediente.ExpedienteArchivos = new List<ExpedienteArchivo>();
            }

            var expedienteArchivos = await this.expedienteArchivoRepository.ListAsync(
                                             new ExpedienteArchivoSpecification(idCandidato))
                                         .ConfigureAwait(false);

            var catalogoExpediente = new List<Expediente>();

            foreach (var expediente in candidatoExpediente.Where(
                e => e.TipoArchivo == ETipoArchivo.CapturaCandidato
                     || e.TipoArchivo == ETipoArchivo.CapturaComplementariaCandidato))
            {
                var item = expedienteArchivos.FirstOrDefault(c => c != null && c.ExpedienteId == expediente.Id);

                if (item != null)
                {
                    expediente.ExpedienteArchivos.Add(item);
                }

                catalogoExpediente.Add(expediente);
            }

            return catalogoExpediente;
        }

        public async Task<List<Expediente>> GetDocumentosPropuesta(int idRequisicion, int idCandidato)
        {
            var expedientes = await this.expedienteRepository.ListAllAsync()
                                  .ConfigureAwait(false);

            var requisicion =
                await this.requisicionDetalleRepository.Single(new RequisicionDetalleSpecification(idRequisicion));

            List<Expediente> documentosPropuesta = null;

            if (requisicion.Propuesta == null)
            {
                requisicion.Propuestas.Add(new RequisicionPropuesta());
                await this.requisicionDetalleRepository.UpdateAsync(requisicion);

                documentosPropuesta = new List<Expediente>();
                foreach (
                var expediente in from expediente in expedientes.Where(c => c.TipoArchivo == ETipoArchivo.Propuesta)
                                  select expediente)
                {
                    documentosPropuesta.Add(expediente);
                }

                return documentosPropuesta;
            }

            var idRequsicionPropuestaId = requisicion.Propuesta.Id;

            var specification = new ExpedienteSpecification(
                idRequisicion,
                idRequsicionPropuestaId,
                ETipoArchivo.Propuesta);
            documentosPropuesta = await this.expedienteRepository.ListAsync(specification)
                                          .ConfigureAwait(false);

            foreach (
                var expediente in from expediente in expedientes.Where(c => c.TipoArchivo == ETipoArchivo.Propuesta)
                                  let item = documentosPropuesta.FirstOrDefault(c => c.Id == expediente.Id)
                                  where item == null
                                  select expediente)
            {
                if (expediente.RequisicionArchivos != null)
                {
                    var archivos = expediente.RequisicionArchivos.ToList();

                    archivos.RemoveAll(r => r.RequisicionPropuestaId != idRequsicionPropuestaId);

                    expediente.RequisicionArchivos = archivos;
                }

                documentosPropuesta.Add(expediente);
            }

            return documentosPropuesta;
        }

        public async Task<List<Expediente>> GetExpedienteAdministrador(int idCandidato)
        {
            var candidatoExpediente = await this.expedienteRepository.ListAllAsync()
                                          .ConfigureAwait(false);

            var expedienteArchivos = await this.expedienteArchivoRepository.ListAsync(
                                             new ExpedienteArchivoSpecification(idCandidato))
                                         .ConfigureAwait(false);

            var catalogoExpediente = new List<Expediente>();

            foreach (var expediente in candidatoExpediente.Where(
                e => e.TipoArchivo == ETipoArchivo.AdministradorExpediente))
            {
                var item = expedienteArchivos.FirstOrDefault(c => c != null && c.ExpedienteId == expediente.Id);

                if (expediente.ExpedienteArchivos == null)
                {
                    expediente.ExpedienteArchivos = new List<ExpedienteArchivo>();
                }

                if (item != null)
                {
                    expediente.ExpedienteArchivos.Add(item);
                }

                catalogoExpediente.Add(expediente);
            }

            return catalogoExpediente;
        }

        public async Task<List<Expediente>> GetExpedienteCandidato(int idCandidato)
        {
            var candidatoExpediente = await this.expedienteRepository.ListAllAsync()
                                          .ConfigureAwait(false);

            var expedienteArchivos = await this.expedienteArchivoRepository.ListAsync(
                                             new ExpedienteArchivoSpecification(idCandidato))
                                         .ConfigureAwait(false);

            var catalogoExpediente = new List<Expediente>();

            foreach (var expediente in candidatoExpediente.Where(e => e.TipoArchivo == ETipoArchivo.Expediente))
            {
                var item = expedienteArchivos.FirstOrDefault(c => c != null && c.ExpedienteId == expediente.Id);

                if (expediente.ExpedienteArchivos == null)
                {
                    expediente.ExpedienteArchivos = new List<ExpedienteArchivo>();
                }

                if (item != null)
                {
                    expediente.ExpedienteArchivos.Add(item);
                }

                catalogoExpediente.Add(expediente);
            }

            return catalogoExpediente;
        }
    }
}
