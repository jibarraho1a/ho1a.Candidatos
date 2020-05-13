using ho1a.applicationCore.Data.Interfaces;
using ho1a.reclutamiento.enums.Candidatos;
using ho1a.reclutamiento.enums.Notificacion;
using ho1a.reclutamiento.enums.Plazas;
using ho1a.reclutamiento.enums.Seguridad;
using ho1a.reclutamiento.models.Candidatos;
using ho1a.reclutamiento.models.Configuracion;
using ho1a.reclutamiento.models.Plazas;
using ho1a.reclutamiento.models.Seguridad;
using ho1a.reclutamiento.services.Data.Interfaces;
using ho1a.reclutamiento.services.Services.Interfaces;
using ho1a.reclutamiento.services.Specifications;
using ho1a.reclutamiento.services.ViewModels.Notificaciones;
using ho1a.reclutamiento.services.ViewModels.Plazas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services
{
    public class CandidatoService : GeneralService<Candidato>, ICandidatoService
    {

        private readonly IAsyncRepository<CandidatoDetalle> candidatoDetalleAsyncRepository;
        private readonly IAsyncRepository<CandidatoExpediente> candidatoExpedienteRepository;
        private readonly IAsyncRepository<Candidato> candidatoRepository;
        private readonly IAsyncRepository<TernaCandidato> candidatoTernaRepository;
        private readonly IGeneralService<PlantillaEntrevista> competenciaService;
        private readonly IGlobalConfiguration<Configuracion> configuracionGlobal;
        private readonly IAsyncRepository<Direccion> direccionRepository;
        private readonly IAsyncRepository<Ingreso> ingresoRepository;
        private readonly INotificarService notificarService;
        private readonly IPlantillaEntrevistaService plantillaEntrevistaService;
        private readonly IAsyncRepository<Prestacion> prestacionRepository;
        private readonly IAsyncRepository<ReferenciaLaboral> referenciaLaboralRepository;
        private readonly IAsyncRepository<ReferenciaPersonal> referenciaPersonalRepository;
        private readonly IAsyncRepository<RequisicionDetalle> requisicionDetalleRepository;
        private readonly IAsyncRepository<Requisicion> requisicionRepository;
        private readonly IRequisicionService requisicionService;
        private readonly IAsyncRepository<Ternas> ternasRepository;
        private readonly IAsyncRepository<UltimoTrabajo> ultimoTrabajoRepository;
        private readonly IUserService userService;
        private ICandidatoService candidatoServiceImplementation;
        
        public CandidatoService(
            IGlobalConfiguration<Configuracion> configuracionGlobal,
            IGeneralService<PlantillaEntrevista> competenciaService,
            IAsyncRepository<Candidato> candidatoAsyncRepository,
            IAsyncRepository<CandidatoDetalle> candidatoDetalleAsyncRepository,
            IAsyncRepository<CandidatoExpediente> candidatoExpedienteRepository,
            IRepository<Candidato> candidatoRepository,
            IAsyncRepository<TernaCandidato> candidatoTernaRepository,
            IAsyncRepository<Ternas> ternasRepository,
            IAsyncRepository<Requisicion> requisicionRepository,
            IAsyncRepository<RequisicionDetalle> requisicionDetalleRepository,
            IAsyncRepository<Direccion> direccionRepository,
            IAsyncRepository<UltimoTrabajo> ultimoTrabajoRepository,
            IAsyncRepository<ReferenciaPersonal> referenciaPersonalRepository,
            IAsyncRepository<ReferenciaLaboral> referenciaLaboralRepository,
            IAsyncRepository<Ingreso> ingresoRepository,
            IAsyncRepository<Prestacion> prestacionRepository,
            INotificarService notificarService,
            IUserService userService)
            : base(candidatoAsyncRepository, candidatoRepository)
        {
            this.configuracionGlobal = configuracionGlobal;
            this.competenciaService = competenciaService;
            this.candidatoRepository = candidatoAsyncRepository;
            this.candidatoDetalleAsyncRepository = candidatoDetalleAsyncRepository;
            this.candidatoExpedienteRepository = candidatoExpedienteRepository;
            this.candidatoTernaRepository = candidatoTernaRepository;
            this.ternasRepository = ternasRepository;
            this.requisicionRepository = requisicionRepository;
            this.requisicionDetalleRepository = requisicionDetalleRepository;
            this.direccionRepository = direccionRepository;
            this.ultimoTrabajoRepository = ultimoTrabajoRepository;
            this.referenciaPersonalRepository = referenciaPersonalRepository;
            this.referenciaLaboralRepository = referenciaLaboralRepository;
            this.ingresoRepository = ingresoRepository;
            this.prestacionRepository = prestacionRepository;
            this.notificarService = notificarService;
            this.userService = userService;
        }

        public async Task<Candidato> AddCandidatoAsync(Candidato candidato)
        {
            try
            {
                if (candidato.CandidatoDetalle == null)
                {
                    candidato.CandidatoDetalle = new CandidatoDetalle();
                }

                if (candidato.CandidatoDetalle.UltimoTrabajo == null)
                {
                    candidato.CandidatoDetalle.UltimoTrabajo = new UltimoTrabajo();
                }

                var detalle = candidato.CandidatoDetalle;
                var ultimoTrabajo = detalle.UltimoTrabajo;

                this.AddPrestaciones(ultimoTrabajo);
                this.AddIngresos(ultimoTrabajo);

                if (candidato.Id == 0)
                {
                    candidato = await this.candidatoRepository.AddAsync(candidato)
                                    .ConfigureAwait(false);
                }
                else
                {
                    await this.candidatoRepository.UpdateAsync(candidato)
                        .ConfigureAwait(false);
                }

                return candidato;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task AddCandidatoToTernaAsync(int idRequisicion, int idCandidato, ETerna idTerna)
        {
            var ternas = await this.ternasRepository.ListAsync(new TernasSpecification(idRequisicion))
                             .ConfigureAwait(false);

            var ternaDestino = ternas.FirstOrDefault(t => t?.TipoTerna == idTerna);

            TernaCandidato ternaCandidato = null;
            foreach (var terna in ternas)
            {
                ternaCandidato = terna.TernaCandidato.FirstOrDefault(tc => tc.Id == idCandidato);

                if (ternaCandidato == null)
                {
                    continue;
                }

                ternaCandidato.Ternas = ternaDestino;
                ternaCandidato.EstadoCandidato = EEstadoCandidato.NoCalificado;
                break;
            }

            if (ternaDestino == null)
            {
                var requisicion = await this.requisicionRepository.Single(new RequisicionSpecification(idRequisicion));

                ternaDestino = new Ternas
                {
                    RequisicionDetalleId = requisicion.RequisicionDetalle.Id,
                    TipoTerna = idTerna
                };

                await this.ternasRepository.AddAsync(ternaDestino);
            }

            if (ternaCandidato != null)
            {
                if (ternaDestino.TernaCandidato == null)
                {
                    ternaDestino.TernaCandidato = new List<TernaCandidato>();
                }

                ternaDestino.TernaCandidato.Add(ternaCandidato);
                await this.ternasRepository.UpdateAsync(ternaDestino)
                    .ConfigureAwait(false);
            }

            await this.AlertarSeguimiento(idRequisicion)
                .ConfigureAwait(false);
        }

        public async Task<Requisicion> AlertarSeguimiento(int idRequisicion)
        {
            var candidatosTernas =
                await this.candidatoTernaRepository.ListAsync(new CandidatoTernaSpecification(idRequisicion));

            var requisicion = await this.requisicionRepository.Single(new RequisicionSpecification(idRequisicion))
                                  .ConfigureAwait(false);

            var hasAlertaSeguimiento = requisicion.RequisicionDetalle.AlertaSeguimeinto != null
                                       && requisicion.RequisicionDetalle.AlertaSeguimeinto.Value;

            var userNameDirectorRH = this.configuracionGlobal.Configuration<string>("UserDireccionRH");
            var alertaSeguimientoTerna = this.configuracionGlobal.Configuration<int>("AlertaSeguimientoTerna") * 3;
            var cancelarSeguimientoTerna = this.configuracionGlobal.Configuration<int>("CancelarSeguimientoTerna") * 3;

            if (candidatosTernas.Count == alertaSeguimientoTerna
                && !hasAlertaSeguimiento)
            {
                requisicion.RequisicionDetalle.AlertaSeguimeinto = true;
                requisicion.RequisicionDetalle.FechaAlertaSeguimiento = DateTime.Now;

                await this.requisicionRepository.UpdateAsync(requisicion)
                    .ConfigureAwait(false);

                await this.NotificarSeguimientoSolicitudAsync(
                        idRequisicion,
                        userNameDirectorRH,
                        ETipoEvento.AlertaSeguimientoTernas)
                    .ConfigureAwait(false);
            }

            if (candidatosTernas.Count != cancelarSeguimientoTerna
                || requisicion?.RequisicionDetalle?.FechaCancelacion != null)
            {
                return requisicion;
            }

            requisicion.RequisicionDetalle.FechaCancelacion = DateTime.Now;

            await this.requisicionRepository.UpdateAsync(requisicion)
                .ConfigureAwait(false);

            await this.NotificarSeguimientoSolicitudAsync(
                    idRequisicion,
                    userNameDirectorRH,
                    ETipoEvento.CancelacionSolicitudLimiteTernas)
                .ConfigureAwait(false);

            return requisicion;
        }

        public async Task<bool> ComplementoExpedienteAsync(int idCandidato, Candidato candidato)
        {
            try
            {
                var toEdit =
                    await this.candidatoRepository.Single(new CandidatoVistaExpedienteSpecification(idCandidato));

                if (candidato.CandidatoDetalle != null)
                {
                    toEdit.CandidatoDetalle.TipoContratacion = candidato.CandidatoDetalle.TipoContratacion;
                    toEdit.CandidatoDetalle.Observaciones = candidato.CandidatoDetalle.Observaciones;
                }

                if (candidato.CandidatoUser != null)
                {
                    toEdit.CandidatoUser.NoColaborador = candidato.CandidatoUser.NoColaborador;
                }

                toEdit.CandidatoDetalle.ProcesoAltaCompleta = candidato.CandidatoDetalle.ProcesoAltaCompleta;

                await this.candidatoRepository.UpdateAsync(toEdit);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<Candidato> GetCandidatoByEmailAsync(string email)
        {
            var result = await this.candidatoRepository.ListAsync(new CandidatoSpecification(email))
                             .ConfigureAwait(false);

            var candidato = result.FirstOrDefault();
            return candidato;
        }

        public async Task<Candidato> GetCandidatoByIdAsync(int idCandidato)
        {
            var result = await this.candidatoRepository.ListAsync(new CandidatoSpecification(idCandidato))
                             .ConfigureAwait(false);

            var candidato = result.FirstOrDefault();

            // var candidato = await this.candidatoRepository.GetByIdAsync(idCandidato)
            // .ConfigureAwait(false);
            return candidato;
        }

        public async Task<Candidato> GetCandidatoByIdAsync(Guid idCandidato)
        {
            var candidato = await this.candidatoRepository.Single(new CandidatoSpecification(idCandidato))
                                .ConfigureAwait(false);

            return candidato;
        }

        public async Task<Candidato> GetCandidatoIdoneoAsync(int idRequisicion)
        {
            var specification = new CandidatoIdoneoSpecification(idRequisicion);
            var result = await this.requisicionDetalleRepository.ListAsync(specification)
                             .ConfigureAwait(false);

            Candidato candidatoIdoneo = null;

            foreach (var requisicionDetalle in result)
            {
                if (requisicionDetalle.Ternas == null)
                {
                    continue;
                }

                foreach (var ternaCandidato in from terna in requisicionDetalle.Ternas
                                               where terna.TernaCandidato != null
                                               from ternaCandidato in terna.TernaCandidato
                                               where ternaCandidato.EstadoCandidato == EEstadoCandidato.Idoneo
                                               select ternaCandidato)
                {
                    candidatoIdoneo = ternaCandidato.Candidato;
                    break;
                }
            }

            return candidatoIdoneo;
        }

        public async Task<List<Candidato>> GetCandidatosByIdRequisicionAsync(int idRequisicion)
        {
            var ternas = await this.ternasRepository.ListAsync(new TernasSpecification(idRequisicion))
                             .ConfigureAwait(false);

            var candidatos = new List<Candidato>();

            foreach (var terna in ternas)
            {
                if (terna.TernaCandidato != null)
                {
                    candidatos.AddRange(terna.TernaCandidato.Select(ternaCandidato => ternaCandidato?.Candidato));
                }
            }

            return candidatos.ToList();
        }

        public async Task<IList<Entrevista>> GetEntrevistasAsync(int idRequisicion)
        {
            var requisicion = await this.requisicionRepository.GetByIdAsync(idRequisicion)
                                  .ConfigureAwait(false);

            var solicitante = await this.userService.GetUserByUserNameAsync(requisicion.UserRequeridor)
                                  .ConfigureAwait(false);
            var userSolicitante = solicitante.UserName;

            // Obtener las plantillas.
            var plantillas = await this.competenciaService.GetAsync(new PlantillaEntrevistaSpecification(idRequisicion))
                                 .ConfigureAwait(false);

            var entrevista1 = new Entrevista
            {
                Entrevistador = requisicion.UserAsignado,
                TipoEntrevista = EEntrevista.Entrevista1,
                Competencias =
                                          plantillas.Select(
                                                  p =>
                                                      new Competencia
                                                      {
                                                          Descripcion = p.Descripcion,
                                                          Nombre = p.Nombre,
                                                          PlantillaEntrevista = p
                                                      })
                                              .ToList()
            };

            var entrevista2 = new Entrevista
            {
                Entrevistador = userSolicitante,
                TipoEntrevista = EEntrevista.Entrevista2,
                Competencias =
                                          plantillas.Select(
                                                  p =>
                                                      new Competencia
                                                      {
                                                          Descripcion = p.Descripcion,
                                                          Nombre = p.Nombre,
                                                          PlantillaEntrevista = p
                                                      })
                                              .ToList()
            };

            var entrevistas = new List<Entrevista> { entrevista1, entrevista2 };

            return entrevistas;
        }

        public async Task<List<TernaCandidato>> GetTernaCandidatoAsync(int idRequisicion)
        {
            var ternasCandidatos = await this.candidatoTernaRepository.ListAsync(
                                           new CandidatoTernaSpecification(idRequisicion))
                                       .ConfigureAwait(false);

            return ternasCandidatos;
        }

        public async Task<List<Ternas>> GetTernasByIdRequisicionAsync(int idRequisicion)
        {
            var result = await this.ternasRepository.ListAsync(new TernasSpecification(idRequisicion))
                             .ConfigureAwait(false);

            return result;
        }

        public async Task<bool> NotificarAsync(int idCandidato, ETipoEvento tipoEvento)
        {
            var candidato = await this.GetCandidatoByIdAsync(idCandidato)
                                .ConfigureAwait(false);

            if (candidato == null)
            {
                return false;
            }

            var notificacion = new NotificacionViewModel
            {
                ToMail = new List<string> { candidato.CandidatoUser.Email },
                TipoEvento = tipoEvento,
                Item = candidato
            };

            return await this.notificarService.NotificarAsync(notificacion)
                       .ConfigureAwait(false);
        }

        public async Task NotificarExpedienteToCandidato(
            int idRequisicion,
            int idCandidato,
            ComentarioViewModel comentario)
        {
            var candidato = await this.GetCandidatoByIdAsync(idCandidato)
                                .ConfigureAwait(false);

            var expediente = candidato?.CandidatoDetalle?.CandidatoExpediente;

            if (expediente != null)
            {
                expediente.CorregirExpediente = comentario.Comentario != null;
                expediente.FechaValidaExpediente = DateTime.Now;
                expediente.FechaNotificacion = DateTime.Now;
                expediente.Comentario = comentario?.Comentario;

                await this.candidatoExpedienteRepository.UpdateAsync(expediente)
                    .ConfigureAwait(false);
            }
        }

        public async Task NotificarExpedienteToRH(int idRequisicion, int idCandidato)
        {
            var candidato = await this.candidatoRepository.Single(new CandidatoSpecification(idCandidato))
                                .ConfigureAwait(false);

            // var candidato = await this.GetCandidatoByIdAsync(idCandidato)
            // .ConfigureAwait(false);
            var expediente = candidato?.CandidatoDetalle?.CandidatoExpediente;

            if (expediente != null)
            {
                // expediente = await this.candidatoExpedienteRepository.GetByIdAsync(expediente.Id).ConfigureAwait(false);
                expediente.FechaNotificacion = DateTime.Now;

                // await this.candidatoExpedienteRepository.UpdateAsync(expediente)
                // .ConfigureAwait(false);
            }

            if (candidato.CandidatoDetalle.Id == 0)
            {
                candidato.CandidatoDetalle.CandidatoId = candidato.Id;
                await this.candidatoDetalleAsyncRepository.AddAsync(candidato.CandidatoDetalle)
                    .ConfigureAwait(false);
            }
            else
            {
                await this.candidatoDetalleAsyncRepository.UpdateAsync(candidato.CandidatoDetalle)
                    .ConfigureAwait(false);
            }

            if (candidato.CandidatoDetalle.CandidatoExpediente.Id == 0)
            {
                await this.candidatoExpedienteRepository.AddAsync(expediente)
                    .ConfigureAwait(false);
            }
            else
            {
                await this.candidatoExpedienteRepository.UpdateAsync(expediente)
                    .ConfigureAwait(false);
            }

            await this.candidatoRepository.UpdateAsync(candidato)
                .ConfigureAwait(false);

            // TODO: Notificar a RH
        }

        public async Task NotificarExpedienteToRH(int idCandidato)
        {
            var expedienteCandidato =
                await this.candidatoExpedienteRepository.Single(new CandidatoExpedienteSpecification(idCandidato));

            var email = expedienteCandidato.CandidatoDetalle.Candidato.CandidatoUser.Email;

            expedienteCandidato.FechaNotificacion = DateTime.Now;
            expedienteCandidato.CorregirExpediente = false;
            await this.candidatoExpedienteRepository.UpdateAsync(expedienteCandidato);

            var administradoresExpediente = this.configuracionGlobal.Configuration<string>(
                    "UsersAdministradorExpediente")
                .Split(';');

            var colaboradores = administradoresExpediente.Select(
                    a =>
                    {
                        return this.userService.GetUserByUserNameAsync(a)
                            .Result;
                    })
                .ToList();

            var notificar = new NotificacionViewModel
            {
                ToMail = colaboradores.Select(x => x.Mail.ToString())
                                        .ToList(),
                TipoEvento = ETipoEvento.NotificarRHCargaExpediente,
                Item =
                                        new
                                        {
                                            expedienteCandidato.CandidatoDetalle.Candidato,
                                            user = colaboradores,
                                            fecha = DateTime.Now
                                        },
                Body = string.Empty
            };

            var service = this.notificarService;
            if (service != null)
            {
                await service.NotificarAsync(notificar)
                    .ConfigureAwait(false);
            }
        }

        public async Task ResetCandidatoIdoneoAsync(int idRequisicion)
        {
            var requisicion = await this.requisicionDetalleRepository.Single(
                                      new CandidatoIdoneoSpecification(idRequisicion))
                                  .ConfigureAwait(false);

            // ternaCandidato in
            var ternaCandidatos = from terna in requisicion.Ternas
                                  where terna.TernaCandidato != null
                                  from ternaCandidato in terna.TernaCandidato
                                  where ternaCandidato.EstadoCandidato == EEstadoCandidato.Idoneo
                                  select ternaCandidato;

            if (ternaCandidatos.Any())
            {
                var toEdit = ternaCandidatos.FirstOrDefault();

                toEdit.Candidato.CandidatoDetalle.StatusSeleccion = EEstadoCandidato.RechazaPropuesta;
                toEdit.EstadoCandidato = EEstadoCandidato.RechazaPropuesta;
                toEdit.Active = false;
                toEdit.Edited = DateTime.Now;

                await this.candidatoTernaRepository.UpdateAsync(toEdit)
                    .ConfigureAwait(false);
            }
        }

        public async Task<bool> SetCargaInformacionAsync(int idCandidato, ECargaInformacionCandidato cargaInformacion)
        {
            try
            {
                var candidato = await this.candidatoRepository.Single(new CandidatoSpecification(idCandidato))
                                    .ConfigureAwait(false);

                if (candidato?.CandidatoDetalle == null)
                {
                    candidato.CandidatoDetalle = new CandidatoDetalle();
                }

                if (cargaInformacion == ECargaInformacionCandidato.Primera)
                {
                    candidato.CandidatoDetalle.StatusCapturaInformacion = EStatusCapturaCandidato.CapturaInicial;
                    candidato.CandidatoDetalle.CargaInformacion1 = DateTime.Now;

                    await this.NotificarAsync(idCandidato, ETipoEvento.NotificarRHCargaInicialInformacion)
                        .ConfigureAwait(false);
                }
                else
                {
                    candidato.CandidatoDetalle.StatusCapturaInformacion = EStatusCapturaCandidato.CapturaComplementaria;
                    candidato.CandidatoDetalle.CargaInformacion2 = DateTime.Now;

                    await this.NotificarAsync(idCandidato, ETipoEvento.NotificarRHCargaComplementariaInformacion)
                        .ConfigureAwait(false);
                }

                await this.candidatoRepository.UpdateAsync(candidato)
                    .ConfigureAwait(false);

                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Candidato> UpdateCandidatoAsync(int idCandidato, Candidato candidato)
        {
            var detalle = candidato.CandidatoDetalle;

            // if (candidato.CandidatoDetalle.StatusCapturaInformacion == EStatusCapturaCandidato.CandidatoNuevo)
            // {
            // candidato.CandidatoDetalle.StatusCapturaInformacion = EStatusCapturaCandidato.NotificarCapturaInicial;
            // }
            if (detalle != null)
            {
                detalle.CandidatoId = candidato.Id;

                if (detalle.Id == 0)
                {
                    await this.candidatoDetalleAsyncRepository.AddAsync(detalle)
                        .ConfigureAwait(false);
                }
                else
                {
                    if (detalle.Direccion != null)
                    {
                        if (detalle.Direccion.Id == 0)
                        {
                            detalle.Direccion.CandidatoDetalleId = detalle.Id;
                            await this.direccionRepository.AddAsync(detalle.Direccion)
                                .ConfigureAwait(false);
                        }
                        else
                        {
                            await this.direccionRepository.UpdateAsync(detalle.Direccion)
                                .ConfigureAwait(false);
                        }
                    }

                    detalle.EstadoCivilId = candidato.CandidatoDetalle.EstadoCivilId;

                    if (detalle.UltimoTrabajo != null)
                    {
                        if (detalle.UltimoTrabajo.Id == 0)
                        {
                            await this.ultimoTrabajoRepository.AddAsync(detalle.UltimoTrabajo)
                                .ConfigureAwait(false);
                        }
                        else
                        {
                            await this.ApplyChangesUltimoTrabajoIngresos(
                                    detalle.UltimoTrabajo.Ingresos,
                                    detalle.UltimoTrabajo)
                                .ConfigureAwait(false);
                            await this.ApplyChangesUltimoTrabajoPrestaciones(
                                    detalle.UltimoTrabajo.Prestaciones,
                                    detalle.UltimoTrabajo)
                                .ConfigureAwait(false);

                            await this.ultimoTrabajoRepository.UpdateAsync(detalle.UltimoTrabajo)
                                .ConfigureAwait(false);
                            await this.candidatoDetalleAsyncRepository.UpdateAsync(detalle)
                                .ConfigureAwait(false);
                        }
                    }

                    await this.ApplyChangesReferenciasLaborales(detalle.ReferenciasLaborales)
                        .ConfigureAwait(false);
                    await this.ApplyChangesReferenciasPersonales(detalle.ReferenciasPersonales)
                        .ConfigureAwait(false);

                    await this.candidatoDetalleAsyncRepository.UpdateAsync(detalle)
                        .ConfigureAwait(false);
                }
            }

            await this.candidatoRepository.UpdateAsync(candidato)
                .ConfigureAwait(false);

            var especificacion = new CandidatoSpecification(idCandidato);
            var result = await this.candidatoRepository.ListAsync(especificacion)
                             .ConfigureAwait(false);
            return result.FirstOrDefault();
        }

        private void AddIngresos(UltimoTrabajo ultimoTrabajo)
        {
            var salarios = Enum.GetValues(typeof(ETipoSalario))
                .Cast<ETipoSalario>();

            foreach (var toInsert in salarios.ToList()
                .Where(p => p != ETipoSalario.OtrosIngresos)
                .Select(prestacion => new Ingreso { TipoIngreso = prestacion }))
            {
                ultimoTrabajo?.Ingresos?.Add(toInsert);
            }
        }

        private void AddPrestaciones(UltimoTrabajo ultimoTrabajo)
        {
            var eTipoPrestaciones = Enum.GetValues(typeof(ETipoPrestacion))
                .Cast<ETipoPrestacion>();

            foreach (var toInsert in eTipoPrestaciones.ToList()
                .Where(p => p != ETipoPrestacion.OtrasPrestaciones)
                .Select(prestacion => new Prestacion { TipoPrestacion = prestacion }))
            {
                ultimoTrabajo?.Prestaciones?.Add(toInsert);
            }
        }

        private async Task ApplyChangesReferenciasLaborales(ICollection<ReferenciaLaboral> detalleReferenciasLaborales)
        {
            foreach (var referenciaLaboral in
                detalleReferenciasLaborales.Where(referenciaLaboral => referenciaLaboral != null))
            {
                if (referenciaLaboral.Id == 0)
                {
                    await this.referenciaLaboralRepository.AddAsync(referenciaLaboral)
                        .ConfigureAwait(false);
                }
                else
                {
                    if (referenciaLaboral.Active)
                    {
                        await this.referenciaLaboralRepository.UpdateAsync(referenciaLaboral)
                            .ConfigureAwait(false);
                    }
                    else
                    {
                        await this.referenciaLaboralRepository.DeleteAsync(referenciaLaboral)
                            .ConfigureAwait(false);
                    }
                }
            }
        }

        private async Task ApplyChangesReferenciasPersonales(
            ICollection<ReferenciaPersonal> detalleReferenciasPersonales)
        {
            foreach (var referenciaPersonal in
                detalleReferenciasPersonales.Where(referenciaPersonal => referenciaPersonal != null))
            {
                if (referenciaPersonal.Id == 0)
                {
                    await this.referenciaPersonalRepository.AddAsync(referenciaPersonal)
                        .ConfigureAwait(false);
                }
                else
                {
                    if (referenciaPersonal.Active)
                    {
                        await this.referenciaPersonalRepository.UpdateAsync(referenciaPersonal)
                            .ConfigureAwait(false);
                    }
                    else
                    {
                        await this.referenciaPersonalRepository.DeleteAsync(referenciaPersonal)
                            .ConfigureAwait(false);
                    }
                }
            }
        }

        private async Task ApplyChangesUltimoTrabajoIngresos(
            ICollection<Ingreso> ultimoTrabajoIngresos,
            UltimoTrabajo detalleUltimoTrabajo)
        {
            foreach (var ingreso in
                ultimoTrabajoIngresos.Where(ingreso => ingreso != null))
            {
                if (ingreso.Id == 0)
                {
                    if (detalleUltimoTrabajo != null)
                    {
                        ingreso.UltimoTrabajoId = detalleUltimoTrabajo.Id;
                    }

                    await this.ingresoRepository.AddAsync(ingreso)
                        .ConfigureAwait(false);
                }
                else
                {
                    if (ingreso.Active)
                    {
                        await this.ingresoRepository.UpdateAsync(ingreso)
                            .ConfigureAwait(false);
                    }
                    else
                    {
                        await this.ingresoRepository.DeleteAsync(ingreso)
                            .ConfigureAwait(false);
                    }
                }
            }
        }

        private async Task ApplyChangesUltimoTrabajoPrestaciones(
            ICollection<Prestacion> ultimoTrabajoPrestaciones,
            UltimoTrabajo detalleUltimoTrabajo)
        {
            foreach (var prestacion in
                ultimoTrabajoPrestaciones.Where(prestacion => prestacion != null))
            {
                if (prestacion.Id == 0)
                {
                    if (detalleUltimoTrabajo != null)
                    {
                        prestacion.UltimoTrabajoId = detalleUltimoTrabajo.Id;
                    }

                    await this.prestacionRepository.AddAsync(prestacion)
                        .ConfigureAwait(false);
                }
                else
                {
                    if (prestacion.Active)
                    {
                        await this.prestacionRepository.UpdateAsync(prestacion)
                            .ConfigureAwait(false);
                    }
                    else
                    {
                        await this.prestacionRepository.DeleteAsync(prestacion)
                            .ConfigureAwait(false);
                    }
                }
            }
        }

        private async Task NotificarSeguimientoSolicitudAsync(
            int idRequisicion,
            string colaboradorToNotify,
            ETipoEvento tipoEvento)
        {
            var colaboradores = new List<User>();
            var userNameColaboradores = colaboradorToNotify?.Split(';');

            if (userNameColaboradores != null)
            {
                foreach (var userNameColaborador in userNameColaboradores)
                {
                    var colaborador = await this.userService.GetUserByUserNameAsync(userNameColaborador)
                                          .ConfigureAwait(false);
                    colaboradores.Add(colaborador);
                }
            }

            if (colaboradores.Any())
            {
                var notificar = new NotificacionViewModel
                {
                    ToMail = colaboradores.Select(x => x.Mail.ToString())
                                            .ToList(),
                    TipoEvento = tipoEvento,
                    Item =
                                            new
                                            {
                                                idRequisicion,
                                                user = colaboradores,
                                                fecha = DateTime.Now
                                            },
                    Body = string.Empty
                };

                var service = this.notificarService;
                if (service != null)
                {
                    await service.NotificarAsync(notificar)
                        .ConfigureAwait(false);
                }
            }
        }

        public async Task<bool> AddResumenCVToCandidatoAsync(int idCandidato, Candidato candidato)
        {
            var especificacion = new CandidatoSpecification(idCandidato);
            var toEdit = await this.candidatoRepository.Single(especificacion)
                             .ConfigureAwait(false);

            if (toEdit == null)
            {
                return false;
            }

            if (candidato != null)
            {
                if (toEdit.CandidatoDetalle != null)
                {
                    if (toEdit.CandidatoDetalle.UltimoTrabajo == null)
                    {
                        toEdit.CandidatoDetalle.UltimoTrabajo = new UltimoTrabajo();
                    }
                    else
                    {
                        var puestoSolicitado = toEdit.CandidatoDetalle.UltimoTrabajo;

                        puestoSolicitado.Empresa = candidato?.CandidatoDetalle?.UltimoTrabajo?.Empresa;
                        puestoSolicitado.Puesto = candidato?.CandidatoDetalle?.UltimoTrabajo.Puesto;
                    }

                    toEdit.CandidatoDetalle.Certificacion = candidato.CandidatoDetalle.Certificacion;
                    toEdit.CandidatoDetalle.PretencionEconomica = candidato.CandidatoDetalle.PretencionEconomica;
                    toEdit.CandidatoDetalle.UltimoSalarioId = candidato.CandidatoDetalle.UltimoSalarioId;
                    toEdit.CandidatoDetalle.Experiencia = candidato.CandidatoDetalle.Experiencia;
                    //toEdit.CandidatoDetalle.PuestoSolicitadoId = candidato.CandidatoDetalle.PuestoSolicitado?.Id;

                    await this.candidatoRepository.UpdateAsync(toEdit);
                    return true;
                }
            }

            return false;
        }

        Task<Candidato> ICandidatoService.AddCandidatoAsync(Candidato candidato)
        {
            throw new NotImplementedException();
        }

        Task ICandidatoService.AddCandidatoToTernaAsync(int idRequisicion, int idCandidato, ETerna idTerna)
        {
            throw new NotImplementedException();
        }

        Task<Requisicion> ICandidatoService.AlertarSeguimiento(int idRequisicion)
        {
            throw new NotImplementedException();
        }

        Task<bool> ICandidatoService.ComplementoExpedienteAsync(int idCandidato, Candidato candidato)
        {
            throw new NotImplementedException();
        }

        Task<Candidato> ICandidatoService.GetCandidatoByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        Task<Candidato> ICandidatoService.GetCandidatoByIdAsync(int idCandidato)
        {
            throw new NotImplementedException();
        }

        Task<Candidato> ICandidatoService.GetCandidatoByIdAsync(Guid idCandidato)
        {
            throw new NotImplementedException();
        }

        Task<Candidato> ICandidatoService.GetCandidatoIdoneoAsync(int idRequisicion)
        {
            throw new NotImplementedException();
        }

        Task<List<Candidato>> ICandidatoService.GetCandidatosByIdRequisicionAsync(int idRequisicion)
        {
            throw new NotImplementedException();
        }

        Task<IList<Entrevista>> ICandidatoService.GetEntrevistasAsync(int idRequisicion)
        {
            throw new NotImplementedException();
        }

        Task<List<TernaCandidato>> ICandidatoService.GetTernaCandidatoAsync(int idRequisicion)
        {
            throw new NotImplementedException();
        }

        Task<bool> ICandidatoService.NotificarAsync(int idCandidato, ETipoEvento tipoEvento)
        {
            throw new NotImplementedException();
        }

        Task ICandidatoService.NotificarExpedienteToCandidato(int idRequisicion, int idCandidato, ComentarioViewModel comentario)
        {
            throw new NotImplementedException();
        }

        Task ICandidatoService.NotificarExpedienteToRH(int idRequisicion, int idCandidato)
        {
            throw new NotImplementedException();
        }

        Task ICandidatoService.NotificarExpedienteToRH(int idCandidato)
        {
            throw new NotImplementedException();
        }

        Task ICandidatoService.ResetCandidatoIdoneoAsync(int idRequisicion)
        {
            throw new NotImplementedException();
        }

        Task<bool> ICandidatoService.SetCargaInformacionAsync(int idCandidato, ECargaInformacionCandidato cargaInformacion)
        {
            throw new NotImplementedException();
        }

        Task<Candidato> ICandidatoService.UpdateCandidatoAsync(int idCandidato, Candidato candidato)
        {
            throw new NotImplementedException();
        }

        Task<bool> ICandidatoService.AddResumenCVToCandidatoAsync(int idCandidato, Candidato candidato)
        {
            throw new NotImplementedException();
        }
    }
}

