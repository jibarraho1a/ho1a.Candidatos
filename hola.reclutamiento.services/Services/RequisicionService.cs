using ho1a.applicationCore.Data.Interfaces;
using ho1a.reclutamiento.enums.Candidatos;
using ho1a.reclutamiento.enums.Notificacion;
using ho1a.reclutamiento.enums.Plazas;
using ho1a.reclutamiento.models.Candidatos;
using ho1a.reclutamiento.models.Configuracion;
using ho1a.reclutamiento.models.Notificacion;
using ho1a.reclutamiento.models.Plazas;
using ho1a.reclutamiento.models.Seguridad;
using ho1a.reclutamiento.services.Data.Interfaces;
using ho1a.reclutamiento.services.Services.Interfaces;
using ho1a.reclutamiento.services.Specifications;
using ho1a.reclutamiento.services.ViewModels.Notificaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services
{
    public class RequisicionService : GeneralService<Requisicion>, IRequisicionService
    {
        private readonly IAutorizacionService autorizacionService;
        private readonly IAsyncRepository<CandidatoDetalle> candidatoDetalleRepository;
        private readonly ICandidatoService candidatoService;
        private readonly IGlobalConfiguration<Configuracion> configuracionGlobal;
        private readonly IAsyncRepository<Configuracion> configuracionRepository;
        private readonly INotificarService notificarService;
        private readonly IAsyncRepository<Requisicion> repository;
        private readonly IAsyncRepository<RequisicionDetalle> requisionCandidatoRepository;
        private readonly IAsyncRepository<Ternas> ternasRepository;
        private readonly IUserService userService;

        public RequisicionService(
            IAsyncRepository<Requisicion> requisicionAsyncRepository,
            IRepository<Requisicion> requisicionRepository,
            IAsyncRepository<RequisicionDetalle> requisionCandidatoRepository,
            IAsyncRepository<Ternas> ternasRepository,
            IAsyncRepository<CandidatoDetalle> candidatoDetalleRepository,
            IAsyncRepository<Configuracion> configuracionRepository,
            IGlobalConfiguration<Configuracion> configuracionGlobal,
            IUserService userService,
            ICandidatoService candidatoService,
            IAutorizacionService autorizacionService,
            INotificarService notificarService)
            : base(requisicionAsyncRepository, requisicionRepository)
        {
            this.repository = requisicionAsyncRepository;
            this.requisionCandidatoRepository = requisionCandidatoRepository;
            this.ternasRepository = ternasRepository;
            this.candidatoDetalleRepository = candidatoDetalleRepository;
            this.configuracionRepository = configuracionRepository;
            this.configuracionGlobal = configuracionGlobal;
            this.userService = userService;
            this.candidatoService = candidatoService;
            this.autorizacionService = autorizacionService;
            this.notificarService = notificarService;
        }

        public async Task AddCandidatoAsync(int idRequisicion, List<Candidato> candidatos, bool isNew = false)
        {
            if (candidatos != null)
            {
                foreach (var candidato in candidatos)
                {
                    await this.AddCandidatoAsync(idRequisicion, candidato).ConfigureAwait(false);

                    var requisicion = await this.candidatoService.AlertarSeguimiento(idRequisicion)
                                          .ConfigureAwait(false);

                    if (requisicion?.RequisicionDetalle?.FechaCancelacion != null)
                    {
                        break;
                    }

                    if (!isNew)
                    {
                        await this.NotificarAsync(candidato, ETipoEvento.InvitarCandidato).ConfigureAwait(false);
                    }
                }

                await this.candidatoService.AlertarSeguimiento(idRequisicion).ConfigureAwait(false);
            }
        }

        public async Task ConfirmarAlta(int idRequisicion, List<Attachment> attachments)
        {
            var specification = new RequisicionDetalleSpecification(idRequisicion);
            var requisicionDetalle = await this.requisionCandidatoRepository.Single(specification)
                                         .ConfigureAwait(false);

            if (requisicionDetalle != null)
            {
                requisicionDetalle.FechaConfirmacionAlta = DateTime.Now;

                // await this.NotificaConfirmacionAlta(requisicionDetalle.RequisicionId)
                // .ConfigureAwait(false);
                await this.NotificaCandidatoAlta(requisicionDetalle.RequisicionId, attachments).ConfigureAwait(false);

                var candidato = await this.candidatoService.GetCandidatoIdoneoAsync(idRequisicion)
                                    .ConfigureAwait(false);

                candidato.CandidatoDetalle.StatusSeleccion = EEstadoCandidato.Colaborador;
                candidato.CandidatoDetalle.RequisicionId = idRequisicion;

                await this.requisionCandidatoRepository.UpdateAsync(requisicionDetalle).ConfigureAwait(false);
            }
        }

        public Task ConfirmarAlta(int idRequisicion, List<System.Net.Mail.Attachment> attachments)
        {
            throw new NotImplementedException();
        }

        public async Task ContestarPropuestaAsync(int idRequisicion, bool aceptaPropuesta, string comentario)
        {
            var specification = new RequisicionDetalleSpecification(idRequisicion);
            var requisicion = await this.requisionCandidatoRepository.Single(specification).ConfigureAwait(false);

            if (requisicion != null)
            {
                var propuesta = requisicion.Propuestas.OrderByDescending(p => p.FechaEnvioPropuesta)
                    .ThenByDescending(p => p.FechaContestacion).FirstOrDefault(
                        p => p.FechaEnvioPropuesta.HasValue && !p.FechaContestacion.HasValue);

                if (propuesta != null)
                {
                    propuesta.PropuestaAceptada = aceptaPropuesta;
                    propuesta.Comentarios = comentario;
                    propuesta.FechaContestacion = DateTime.Now;
                }

                if (!aceptaPropuesta)
                {
                    requisicion.Propuestas.Add(new RequisicionPropuesta());
                }

                await this.requisionCandidatoRepository.UpdateAsync(requisicion).ConfigureAwait(false);

                await this.SetRequisicionContestacionCandidato(requisicion, aceptaPropuesta).ConfigureAwait(false);
            }
        }

        public async Task EnviarPropuestaAsync(int idRequisicion, List<Attachment> attachments)
        {
            var specification = new RequisicionDetalleSpecification(idRequisicion);
            var requisicion = await this.requisionCandidatoRepository.Single(specification).ConfigureAwait(false);

            if (requisicion != null)
            {
                var propuesta = requisicion.Propuestas.OrderByDescending(p => p.Created)
                    .ThenByDescending(p => p.FechaEnvioPropuesta).ThenByDescending(p => p.FechaContestacion)
                    .FirstOrDefault(p => !p.FechaEnvioPropuesta.HasValue && !p.FechaContestacion.HasValue);

                if (propuesta == null)
                {
                    propuesta = new RequisicionPropuesta { FechaEnvioPropuesta = DateTime.Now };
                    requisicion.Propuestas.Add(propuesta);
                }
                else
                {
                    propuesta.FechaEnvioPropuesta = DateTime.Now;
                }

                await this.requisionCandidatoRepository.UpdateAsync(requisicion).ConfigureAwait(false);

                var candidato = await this.candidatoService.GetCandidatoIdoneoAsync(idRequisicion)
                                    .ConfigureAwait(false);
                candidato = this.candidatoService.Single(new CandidatoSpecification(candidato.Id));

                await this.NotificarOfertaEconomica(candidato, attachments).ConfigureAwait(false);
            }
        }

        public Task EnviarPropuestaAsync(int idRequisicion, List<System.Net.Mail.Attachment> attachments)
        {
            throw new NotImplementedException();
        }

        public async Task EstablecerFechaIngresoAsync(int idRequisicion, DateTime fechaIngreso)
        {
            var specification = new RequisicionDetalleSpecification(idRequisicion);
            var requisicion = await this.requisionCandidatoRepository.Single(specification).ConfigureAwait(false);

            if (requisicion != null)
            {
                requisicion.FechaIngreso = fechaIngreso;

                await this.requisionCandidatoRepository.UpdateAsync(requisicion).ConfigureAwait(false);

                var candidato = await this.candidatoService.GetCandidatoIdoneoAsync(idRequisicion)
                                    .ConfigureAwait(false);

                await this.NotificarAsync(candidato, ETipoEvento.NotificarCandidatoFechaIngreso, requisicion)
                    .ConfigureAwait(false);
            }
        }

        public async Task EstablecerSalarioPropuesto(int idRequisicion, dynamic propuestaSalarial)
        {
            var asyncRepository = this.repository;
            if (asyncRepository != null)
            {
                var requisicion = await asyncRepository.Single(new RequisicionSpecification(idRequisicion))
                                      .ConfigureAwait(false);

                if (requisicion != null)
                {
                    RequisicionPropuesta propuesta = null;
                    var detalle = requisicion.RequisicionDetalle;
                    if (detalle != null)
                    {
                        propuesta = detalle.Propuestas.OrderByDescending(p => p.Created)
                            .ThenByDescending(p => p.FechaEnvioPropuesta).ThenByDescending(p => p.FechaContestacion)
                            .FirstOrDefault(p => !p.FechaEnvioPropuesta.HasValue && !p.FechaContestacion.HasValue);

                        if (propuesta == null)
                        {
                            propuesta = new RequisicionPropuesta
                            {
                                Salario = propuestaSalarial.salario,
                                Bonos = propuestaSalarial.bonos,
                                Beneficios = propuestaSalarial.beneficios
                            };
                            detalle.Propuestas.Add(propuesta);
                        }
                        else
                        {
                            propuesta.Salario = propuestaSalarial.salario;
                            propuesta.Bonos = propuestaSalarial.bonos;
                            propuesta.Beneficios = propuestaSalarial.beneficios;
                        }
                    }

                    await UpdateAsync(idRequisicion, requisicion).ConfigureAwait(false);

                    // Notificación de propuesta.
                    await this.NotificarPropuestaCargada(idRequisicion).ConfigureAwait(false);

                    if (propuesta != null)
                    {
                        await this.ValidarTopeSalarial(requisicion, propuesta.Salario).ConfigureAwait(false);
                    }
                }
            }
        }

        public async Task EstablecerUsuarioExpediente(int idRequisicion, string userAd)
        {
            var asyncRepository = this.repository;
            if (asyncRepository != null)
            {
                var requisicion = await asyncRepository.Single(new RequisicionSpecification(idRequisicion))
                                      .ConfigureAwait(false);

                if (requisicion != null)
                {
                    var candidato = await this.candidatoService.GetCandidatoIdoneoAsync(requisicion.Id)
                                        .ConfigureAwait(false);

                    candidato.CandidatoUser.UserAd = userAd;

                    // candidato.CandidatoDetalle.StatusSeleccion = EEstadoCandidato.Colaborador;
                    await this.UpdateAsync(idRequisicion, requisicion).ConfigureAwait(false);
                }
            }
        }

        public async Task<List<Ternas>> GetTernasByRequisicionAsync(int idRequisicion)
        {
            var specification = new RequisicionDetalleSpecification(idRequisicion);
            var result = await this.requisionCandidatoRepository.ListAsync(specification).ConfigureAwait(false);

            var ternas = new List<Ternas>();

            foreach (var detalle in result)
            {
                if (detalle?.Ternas == null)
                {
                    continue;
                }

                foreach (var terna in detalle.Ternas)
                {
                    terna.RequisicionDetalle.Requisicion.PuestoSolicitado = detalle.Requisicion.PuestoSolicitado;
                }

                ternas.AddRange(detalle.Ternas);
            }

            return ternas;
        }

        public async Task NotificarAltaAsync(int idRequisicion)
        {
            var specification = new RequisicionDetalleSpecification(idRequisicion);
            var requisicionDetalle = await this.requisionCandidatoRepository.Single(specification)
                                         .ConfigureAwait(false);

            if (requisicionDetalle != null)
            {
                requisicionDetalle.FechaNotificacionAlta = DateTime.Now;

                var candidato = await this.candidatoService.GetCandidatoIdoneoAsync(requisicionDetalle.RequisicionId)
                                    .ConfigureAwait(false);

                await this.requisionCandidatoRepository.UpdateAsync(requisicionDetalle).ConfigureAwait(false);

                await this.NotificarAlta(requisicionDetalle.RequisicionId, candidato).ConfigureAwait(false);
            }
        }

        public async Task<User> SetAsignacionAsync(
            Requisicion requisicion,
            string userAsignacion = null,
            bool estableceInicioReclutamiento = true)
        {
            User user = null;
            var asyncRepository = this.repository;
            if (asyncRepository == null)
            {
                return null;
            }

            if (requisicion == null)
            {
                return user;
            }

            if (!requisicion.ValidaRequisiciones.All(v => v.EstadoValidacion == EEstadoValidacion.Aprobada))
            {
                return user;
            }

            var userAsignado = string.IsNullOrEmpty(userAsignacion)
                                   ? this.configuracionGlobal?.Configuration<string>("UserCoordinadorRS")
                                   : userAsignacion;

            user = await this.userService.GetUserByUserNameAsync(userAsignado).ConfigureAwait(false);

            if (user == null)
            {
                return user;
            }

            requisicion.UserAsignado = userAsignado;

            requisicion.Reasignaciones?.Add(
                new Reasignacion { FechaInicio = DateTime.Now, UserNameTo = requisicion.UserAsignado });

            if (estableceInicioReclutamiento && requisicion.InicioReclutamiento == null)
            {
                requisicion.InicioReclutamiento = DateTime.Now;
            }

            requisicion.FechaSolicitud = requisicion.FechaSolicitud ?? DateTime.Now;

            this.AddTernas(requisicion);

            await this.UpdateAsync(requisicion.Id, requisicion).ConfigureAwait(false);

            await this.NotificarAsignacion(requisicion.Id, user.UserName).ConfigureAwait(false);

            return user;
        }

        public async Task<User> SetAsignacionAsync(
            int idRequisicion,
            string userAsignacion = null,
            bool estableceInicioReclutamiento = true)
        {
            var requisicion = await this.repository.Single(new RequisicionSpecification(idRequisicion))
                                  .ConfigureAwait(false);

            return await this.SetAsignacionAsync(requisicion, userAsignacion, estableceInicioReclutamiento)
                       .ConfigureAwait(false);
        }

        public async Task SetCandidatoIdoneo(int idRequisicion, int idCandidato, EEstadoCandidato valor)
        {
            var ternas = await this.GetTernasByRequisicionAsync(idRequisicion).ConfigureAwait(false);

            foreach (var terna in ternas)
            {
                if (terna.TernaCandidato != null)
                {
                    foreach (var ternaCandidato in terna.TernaCandidato)
                    {
                        if (valor != EEstadoCandidato.Idoneo)
                        {
                            if (ternaCandidato.CandidatoId != idCandidato)
                            {
                                continue;
                            }

                            ternaCandidato.EstadoCandidato = valor;

                            if (valor < 0)
                            {
                                ternaCandidato.TernasId = ternas.FirstOrDefault(t => t.TipoTerna == ETerna.SinDefinir)
                                    .Id;
                            }

                            if (ternaCandidato.Candidato?.CandidatoDetalle == null)
                            {
                                continue;
                            }

                            ternaCandidato.Candidato.CandidatoDetalle.StatusSeleccion = valor;

                            break;
                        }

                        if (ternaCandidato.CandidatoId == idCandidato)
                        {
                            ternaCandidato.EstadoCandidato = EEstadoCandidato.Idoneo;
                            ternaCandidato.Candidato.CandidatoDetalle.StatusCapturaInformacion =
                                EStatusCapturaCandidato.NotificarCapturaComplementaria;
                        }
                        else
                        {
                            if (ternaCandidato.EstadoCandidato == EEstadoCandidato.NoCalificado)
                            {
                                ternaCandidato.EstadoCandidato = EEstadoCandidato.NoIdoneo;
                            }

                            if (ternaCandidato.EstadoCandidato == EEstadoCandidato.Idoneo
                                && ternaCandidato.CandidatoId != idCandidato)
                            {
                                ternaCandidato.EstadoCandidato = EEstadoCandidato.NoIdoneo;
                            }
                        }
                    }
                }

                await this.ternasRepository.UpdateAsync(terna).ConfigureAwait(false);
            }

            if (valor <= EEstadoCandidato.NoCalificado)
            {
                return;
            }

            var candidato = this.candidatoService.Single(new CandidatoSpecification(idCandidato));

            await this.NotificarSeleccionCandidato(idRequisicion, candidato).ConfigureAwait(false);
            await this.NotificarAsync(candidato, ETipoEvento.NotificarCandidatoCargaComplementariaInformacion)
                .ConfigureAwait(false);
        }

        public async Task SetInicioRequisicionAsync(int idRequisicion)
        {
            var asyncRepository = this.repository;
            if (asyncRepository != null)
            {
                var result = await asyncRepository.ListAsync(new RequisicionSpecification(idRequisicion))
                                 .ConfigureAwait(false);

                var requisicion = result.FirstOrDefault();

                if (requisicion != null)
                {
                    requisicion.FechaSolicitud = DateTime.Now;

                    await this.UpdateAsync(idRequisicion, requisicion).ConfigureAwait(false);
                }
            }
        }

        public async Task SetTipoBusqueda(int idRequisicion, ETipoBusqueda idTipoBusqueda)
        {
            var asyncRepository = this.repository;
            if (asyncRepository != null)
            {
                var requisicion = await asyncRepository.Single(new RequisicionSpecification(idRequisicion))
                                      .ConfigureAwait(false);

                if (requisicion != null)
                {
                    requisicion.TipoBusqueda = idTipoBusqueda;

                    await this.UpdateAsync(idRequisicion, requisicion).ConfigureAwait(false);

                    if (idTipoBusqueda == ETipoBusqueda.Interna)
                    {
                        await this.NotificarTipoBusqueda(idRequisicion).ConfigureAwait(false);
                    }
                }
            }
        }

        private async Task AddCandidatoAsync(int idRequisicion, Candidato candidato)
        {
            var entrevistas = await this.candidatoService.GetEntrevistasAsync(idRequisicion).ConfigureAwait(false);

            foreach (var entrevista in entrevistas)
            {
                entrevista.CandidatoId = candidato.Id;
            }

            candidato = this.candidatoService.Single(new CandidatoSpecification(candidato.Id));
            candidato.CandidatoDetalle.StatusCapturaInformacion = EStatusCapturaCandidato.NotificarCapturaInicial;

            await this.candidatoService.UpdateAsync(candidato.Id, candidato).ConfigureAwait(false);

            if (candidato != null)
            {
                var ternaCandidato = new TernaCandidato { CandidatoId = candidato.Id, Entrevistas = entrevistas };

                var terna = await this.AddTernaCandidatoAsync(idRequisicion).ConfigureAwait(false);

                if (terna.TernaCandidato != null)
                {
                    terna.TernaCandidato.Add(ternaCandidato);
                }
                else
                {
                    terna.TernaCandidato = new List<TernaCandidato> { ternaCandidato };
                }

                await this.ternasRepository.UpdateAsync(terna).ConfigureAwait(false);

                await this.candidatoService.AlertarSeguimiento(idRequisicion).ConfigureAwait(true);
            }
        }

        private async Task<Ternas> AddTernaCandidatoAsync(int idRequisicion)
        {
            var ternas = await this.ternasRepository.ListAsync(new TernasSpecification(idRequisicion))
                             .ConfigureAwait(false);

            var ternasComplete = ternas.Where(t => t?.TipoTerna != ETerna.SinDefinir).Select(
                t => new { t.Id, IsComplete = t.TernaCandidato?.Any() == true && t.TernaCandidato.Count == 3 });

            Ternas terna = null;

            var ternaLibre = ternasComplete.FirstOrDefault(t => !t.IsComplete);
            if (ternaLibre != null)
            {
                terna = ternas.FirstOrDefault(t => t?.Id == ternaLibre.Id);
            }
            else
            {
                var resultRequisicion = await this.GetAsync(new RequisicionSpecification(idRequisicion))
                                            .ConfigureAwait(false);
                var detalle = resultRequisicion.FirstOrDefault()?.RequisicionDetalle;
                var ternaRepository = this.ternasRepository;
                if (ternas.Count == 0)
                {
                    var ternaSinDefinir = new Ternas
                    {
                        TipoTerna = ETerna.SinDefinir,
                        RequisicionDetalleId = detalle?.Id
                    };

                    var nextTerna = new Ternas { TipoTerna = ETerna.Terna1, RequisicionDetalleId = detalle?.Id };

                    if (ternaRepository == null)
                    {
                        return null;
                    }

                    if (ternaRepository != null)
                    {
                        await ternaRepository.AddAsync(ternaSinDefinir).ConfigureAwait(false);

                        await ternaRepository.AddAsync(nextTerna).ConfigureAwait(false);

                        terna = nextTerna;
                    }
                }
                else
                {
                    var nextTerna = new Ternas
                    {
                        // TernaCandidato = new List<TernaCandidato> { candidatoTerna },
                        TipoTerna = (ETerna)ternas.Count,
                        RequisicionDetalleId = detalle?.Id
                    };
                    if (ternaRepository == null)
                    {
                        return null;
                    }

                    await ternaRepository.AddAsync(nextTerna).ConfigureAwait(false);

                    terna = nextTerna;
                }
            }

            ternas = await this.ternasRepository.ListAsync(new TernasSpecification(idRequisicion))
                         .ConfigureAwait(false);

            return ternas.FirstOrDefault(t => t.Id == terna.Id);
        }

        private void AddTernas(Requisicion requisicion)
        {
            if (requisicion.InicioReclutamiento == null)
            {
                return;
            }

            if (requisicion.RequisicionDetalle == null)
            {
                requisicion.RequisicionDetalle = new RequisicionDetalle();
            }

            if (requisicion.RequisicionDetalle.Ternas == null)
            {
                requisicion.RequisicionDetalle.Ternas = new List<Ternas>
                                                            {
                                                                new Ternas
                                                                    {
                                                                       TipoTerna = ETerna.SinDefinir
                                                                    },
                                                                new Ternas
                                                                    {
                                                                       TipoTerna = ETerna.Terna1
                                                                    }
                                                            };
            }
        }

        private async Task NotificaCandidatoAlta(int idRequisicion, List<Attachment> attachments)
        {
            var candidato = await this.candidatoService.GetCandidatoIdoneoAsync(idRequisicion).ConfigureAwait(false);

            candidato = this.candidatoService.Single(new CandidatoSpecification(candidato.Id));

            var notificar = new NotificacionViewModel
            {
                ToMail = new List<string>
                                                 {
                                                    candidato.CandidatoUser.Email
                                                 },
                TipoEvento = ETipoEvento.NotificarCandidatoAlta,
                Item = new { candidato },
                Body = string.Empty,
                Attachments = attachments
            };

            var service = this.notificarService;
            if (service != null)
            {
                await service.NotificarAsync(notificar).ConfigureAwait(false);
            }
        }

        private async Task NotificaConfirmacionAlta(int idRequisicion)
        {
            var requisicion = await this.repository.Single(new RequisicionSpecification(idRequisicion))
                                  .ConfigureAwait(false);
            var candidato = await this.candidatoService.GetCandidatoIdoneoAsync(idRequisicion).ConfigureAwait(false);

            candidato = this.candidatoService.Single(new CandidatoSpecification(candidato.Id));

            var notificar = new NotificacionViewModel
            {
                ToMail = new List<string>
                                                 {
                                                    candidato.CandidatoUser.Email
                                                 },
                TipoEvento = ETipoEvento.NotificarConfirmacionAlta,
                Item = new { requisicion, candidato },
                Body = string.Empty
            };

            var service = this.notificarService;
            if (service != null)
            {
                await service.NotificarAsync(notificar).ConfigureAwait(false);
            }
        }

        private async Task NotificarAlta(int idRequisicion, Candidato candidato)
        {
            var canditoToSend = this.candidatoService.Single(new CandidatoSpecification(candidato.Id));
            var requisicion = await this.repository.Single(new RequisicionSpecification(idRequisicion))
                                  .ConfigureAwait(false);

            var configuraciones = await this.configuracionRepository.ListAllAsync().ConfigureAwait(false);

            var itemToFind = new List<string>
                                 {
                                     "UserBussinesPartnerRH",
                                     "UserEspecialistaContabilidad",
                                     "UserAltaExpense",
                                     "UserAltaBeneficiosFlexibles",
                                     "UserAsesorRS",
                                     "UserAsignacionLugarEstacionamiento",
                                     "UserEntregaTarjetaAccesoOficina",
                                     "UserEntregaTarjetaAccesoEdificio",
                                     "UserEntregaTarjetaBancaria",
                                     "UserTI"
                                 };

            foreach (var keyConfiguracion in itemToFind)
            {
                var configuracion = configuraciones.FirstOrDefault(c => c.Key == keyConfiguracion);

                var usuarios = configuracion.Values.Split(';').ToList();

                var listUsuario = new List<User>();

                foreach (var usuario in usuarios)
                {
                    var _user = await this.userService.GetUserByUserNameAsync(usuario);
                    listUsuario.Add(_user);
                }

                listUsuario = listUsuario.Where(u => u != null).ToList();

                if (!listUsuario.Any())
                {
                    continue;
                }

                var notificar = new NotificacionViewModel
                {
                    ToMail = listUsuario.Select(u => u.Mail).ToList(),
                    TipoEvento = ETipoEvento.NotificarAltaColaborador,
                    Item = new
                    {
                        candidato = canditoToSend,
                        requisicion,
                        user = listUsuario,
                        actividades =
                                                           configuracion.Values2
                                                               .Split(';').ToList()
                    },
                    Body = string.Empty
                };

                var service = this.notificarService;
                if (service != null)
                {
                    await service.NotificarAsync(notificar).ConfigureAwait(false);
                }
            }
        }

        private async Task NotificarAsignacion(int idRequisicion, string colaboradorUserName)
        {
            var colaborador = await this.userService.GetUserByUserNameAsync(colaboradorUserName).ConfigureAwait(false);

            var notificar = new NotificacionViewModel
            {
                ToMail = new List<string> { colaborador.Mail },
                TipoEvento = ETipoEvento.NotificarAsignacion,
                Item = new
                {
                    idRequisicion,
                    user = colaborador,
                    fecha = DateTime.Now
                },
                Body = string.Empty
            };

            var service = this.notificarService;
            if (service != null)
            {
                await service.NotificarAsync(notificar).ConfigureAwait(false);
            }
        }

        private async Task NotificarAsync(
            Candidato candidato,
            ETipoEvento tipoEvento,
            RequisicionDetalle requisicion = null)
        {
            if (candidato.CandidatoUser == null)
            {
                candidato = await this.candidatoService.GetCandidatoByIdAsync(candidato.Id).ConfigureAwait(false);
            }

            if (candidato.CandidatoUser == null)
            {
                return;
            }

            var notificar = new NotificacionViewModel
            {
                ToMail = new List<string>
                                                 {
                                                    candidato.CandidatoUser.Email
                                                 },
                TipoEvento = tipoEvento,
                Item = candidato,
                Body = string.Empty
            };

            if (requisicion == null)
            {
                notificar.Item = candidato;
            }
            else
            {
                notificar.Item = new { candidato, requisicion };
            }

            var service = this.notificarService;
            if (service != null)
            {
                await service.NotificarAsync(notificar).ConfigureAwait(false);
            }
        }

        private async Task NotificarOfertaEconomica(Candidato candidato, List<Attachment> attachments)
        {
            // Obtener los archivos a adjuntar.
            // Adjuntar los archivos.
            if (string.IsNullOrEmpty(candidato?.CandidatoUser?.Email))
            {
                throw new Exception("Candidato debe de contar con correo electronico");
            }

            var notificar = new NotificacionViewModel
            {
                ToMail = new List<string>
                                                 {
                                                    candidato.CandidatoUser.Email
                                                 },
                TipoEvento = ETipoEvento.EnviarOferta,
                Item = new { candidato },
                Body = string.Empty,
                Attachments = attachments
            };

            var service = this.notificarService;
            if (service != null)
            {
                await service.NotificarAsync(notificar).ConfigureAwait(false);
            }
        }

        private async Task NotificarPropuestaAceptadaAsync(RequisicionDetalle requisicion)
        {
            var candidato = await this.candidatoService.GetCandidatoIdoneoAsync(requisicion.RequisicionId)
                                .ConfigureAwait(false);

            var listUsuario = new List<User>();

            var userNameAdministradorExpediente =
                this.configuracionGlobal.Configuration<string>("UsersAdministradorExpediente");

            foreach (var item in userNameAdministradorExpediente.Split(";"))
            {
                var user = await this.userService.GetUserByUserNameAsync(item);
                listUsuario.Add(user);
            }

            var notificar = new NotificacionViewModel
            {
                ToMail = listUsuario.Select(u => u.Mail).ToList(),
                TipoEvento = ETipoEvento.NotificarPropuestaAceptada,
                Item = new
                {
                    candidato,
                    requisicion = requisicion.Requisicion,
                    user = listUsuario
                },
                Body = string.Empty
            };

            var service = this.notificarService;
            if (service != null)
            {
                await service.NotificarAsync(notificar).ConfigureAwait(false);
            }
        }

        private async Task NotificarPropuestaCargada(int idRequisicion)
        {
            var requisicion = await this.repository.Single(new RequisicionSpecification(idRequisicion))
                                  .ConfigureAwait(false);

            var candidato = await this.candidatoService.GetCandidatoIdoneoAsync(requisicion.Id).ConfigureAwait(false);

            var listUsuario = new List<User>();
            var _user = await this.userService.GetUserByUserNameAsync(requisicion.UserAsignado);
            listUsuario.Add(_user);

            var notificar = new NotificacionViewModel
            {
                ToMail = listUsuario.Select(u => u.Mail).ToList(),
                TipoEvento = ETipoEvento.NotificarCargaPropuesta,
                Item = new
                {
                    candidato,
                    requisicion,
                    user = listUsuario
                },
                Body = string.Empty
            };

            var service = this.notificarService;
            if (service != null)
            {
                await service.NotificarAsync(notificar).ConfigureAwait(false);
            }
        }

        private async Task NotificarSeleccionCandidato(int idRequisicion, Candidato candidato)
        {
            var userNameRS = this.configuracionGlobal.Configuration<string>("NotificarCandidatoIdoneo");

            foreach (var user in userNameRS.Split(';').ToList())
            {
                User userToSend = null;
                if (user.Contains("@"))
                {
                    userToSend = new User
                    {
                        Mail = user,
                        Nombre = user.Split('@')[0]
                    };
                }
                else
                {
                    userToSend = await this.userService.GetUserByUserNameAsync(user).ConfigureAwait(false);
                }

                await this.SendNotificacion(idRequisicion, candidato, userToSend);
            }
        }

        private async Task NotificarTipoBusqueda(int idRequisicion)
        {
            var userNameRS = this.configuracionGlobal.Configuration<string>("CoordinadorCulturaTalento");
            var userRS = await this.userService.GetUserByUserNameAsync(userNameRS).ConfigureAwait(false);

            var notificar = new NotificacionViewModel
            {
                ToMail = new List<string> { userRS.Mail },
                TipoEvento = ETipoEvento.PublicacionInterna,
                Item = new { idRequisicion, user = userRS },
                Body = string.Empty
            };

            var service = this.notificarService;
            if (service != null)
            {
                await service.NotificarAsync(notificar).ConfigureAwait(false);
            }
        }

        private async Task ResetearPropuestaRequisicion(RequisicionDetalle requisicion, bool aceptaPropuesta)
        {
            if (aceptaPropuesta)
            {
                return;
            }

            if (requisicion != null)
            {
                await this.candidatoService.ResetCandidatoIdoneoAsync(requisicion.RequisicionId).ConfigureAwait(false);
            }
        }

        private async Task SendNotificacion(int idRequisicion, Candidato candidato, User userToSend)
        {
            var notificar = new NotificacionViewModel
            {
                ToMail = new List<string> { userToSend.Mail },
                TipoEvento = ETipoEvento.NotificarSeleccionCandidato,
                Item = new
                {
                    idRequisicion,
                    user = userToSend,
                    candidato,
                    fecha = DateTime.Now
                },
                Body = string.Empty
            };

            var service = this.notificarService;
            if (service != null)
            {
                await service.NotificarAsync(notificar).ConfigureAwait(false);
            }
        }

        private async Task SetRequisicionContestacionCandidato(RequisicionDetalle requisicion, bool aceptaPropuesta)
        {
            var statusPropuesta = aceptaPropuesta
                                      ? EEstadoCandidato.AceptaPropuesta
                                      : EEstadoCandidato.RechazaPropuesta;

            Candidato candidato = null;
            if (requisicion != null)
            {
                candidato = await this.candidatoService.GetCandidatoIdoneoAsync(requisicion.Id).ConfigureAwait(false);
                if (candidato?.CandidatoDetalle != null)
                {
                    candidato.CandidatoDetalle.StatusSeleccion = statusPropuesta;
                }

                if (statusPropuesta == EEstadoCandidato.AceptaPropuesta)
                {
                    await this.NotificarPropuestaAceptadaAsync(requisicion);
                }

                await this.ResetearPropuestaRequisicion(requisicion, aceptaPropuesta).ConfigureAwait(false);
            }

            if (candidato != null)
            {
                await this.candidatoService.UpdateCandidatoAsync(candidato.Id, candidato).ConfigureAwait(false);
            }
        }

        private async Task ValidarTopeSalarial(Requisicion requisicion, decimal? propuestaSalario)
        {
            var tabulador = requisicion.TabuladorSalario;
            //var notificarDireccionRH = false;
            //var notificarDireccionGeneral = true;

            var topeSalarialRhRechazado = requisicion.ValidaRequisiciones.FirstOrDefault(
                v => v != null && v.NivelValidacion == ENivelValidacion.TopeSalarialDireccionRH
                               && v.EstadoValidacion == EEstadoValidacion.Rechazada);

            var topeSalarialDgRechazado = requisicion.ValidaRequisiciones.FirstOrDefault(
                v => v != null && v.NivelValidacion == ENivelValidacion.TopeSalarialDireccionGeneral
                               && v.EstadoValidacion == EEstadoValidacion.Rechazada);

            if (topeSalarialRhRechazado != null)
            {
                topeSalarialRhRechazado.EstadoValidacion = EEstadoValidacion.Actual;
                await this.autorizacionService.AprobacionTopeSalarialAsync(
                    requisicion.Id,
                    topeSalarialRhRechazado.AprobadorUserName,
                    topeSalarialRhRechazado);

                // TODO: Notificar
                return;
            }

            if (topeSalarialDgRechazado != null)
            {
                topeSalarialDgRechazado.EstadoValidacion = EEstadoValidacion.Actual;
                await this.autorizacionService.AprobacionTopeSalarialAsync(
                    requisicion.Id,
                    topeSalarialDgRechazado.AprobadorUserName,
                    topeSalarialDgRechazado);

                // TODO: Notificar
                return;
            }

            if (tabulador != null)
            {
                var montoMaximoDireccionGeneral = tabulador.Maximo * 100 / 90;

                if (propuestaSalario.Value > tabulador.Maximo)
                {
                    var userDireccionRH = this.configuracionGlobal.Configuration<string>("UserDireccionRH");

                    // El validador es el primer usuario que se encuentra.
                    if (userDireccionRH.Split(';').Count() > 1)
                    {
                        userDireccionRH = userDireccionRH.Split(';')[0];
                    }

                    await this.autorizacionService.AddValidacionAsync(
                        requisicion.Id,
                        requisicion.UserRequeridor,
                        userDireccionRH,
                        requisicion.MotivoIngreso.Descripcion,
                        ENivelValidacion.TopeSalarialDireccionRH).ConfigureAwait(false);
                }

                if (propuestaSalario.Value > montoMaximoDireccionGeneral)
                {
                    var userDireccionGeneral = this.configuracionGlobal.Configuration<string>("UserDireccionGeneral");
                    await this.autorizacionService.AddValidacionAsync(
                        requisicion.Id,
                        requisicion.UserRequeridor,
                        userDireccionGeneral,
                        requisicion.MotivoIngreso.Descripcion,
                        ENivelValidacion.TopeSalarialDireccionGeneral).ConfigureAwait(false);
                }
            }
        }
    }
}
