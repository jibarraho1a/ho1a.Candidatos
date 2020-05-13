using ho1a.Api.HttpClient;
using ho1a.applicationCore.Data.Interfaces;
using ho1a.applicationCore.Utilerias;
using ho1a.reclutamiento.enums.Notificacion;
using ho1a.reclutamiento.models.Candidatos;
using ho1a.reclutamiento.models.Catalogos;
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
    public class NotificacionService : INotificarService
    {
        private readonly IAsyncRepository<Candidato> candidatoRepository;
        private readonly IGlobalConfiguration<Configuracion> configuration;
        private readonly IAsyncRepository<Entrevista> entrevistaRepository;
        private readonly IAsyncRepository<Localidad> localidadRepository;
        private readonly IAsyncRepository<NotificacionCorreos> notificacionCorreosRepository;
        private readonly IAsyncRepository<Requisicion> requisicionRepository;
        private readonly IUserService userService;

        public NotificacionService(
            IGlobalConfiguration<Configuracion> configuration,
            IAsyncRepository<Requisicion> requisicionRepository,
            IAsyncRepository<Entrevista> entrevistaRepository,
            IAsyncRepository<Candidato> candidatoRepository,
            IAsyncRepository<Localidad> localidadRepository,
            IUserService userService,
            IAsyncRepository<NotificacionCorreos> notificacionCorreosRepository)
        {
            this.configuration = configuration;
            this.requisicionRepository = requisicionRepository;
            this.entrevistaRepository = entrevistaRepository;
            this.candidatoRepository = candidatoRepository;
            this.localidadRepository = localidadRepository;
            this.userService = userService;
            this.notificacionCorreosRepository = notificacionCorreosRepository;
        }

        public async Task<bool> NotificarAsync(NotificacionViewModel notificacionViewModel)
        {
            try
            {
                var globalConfiguration = this.configuration;
                if (globalConfiguration == null)
                {
                    return false;
                }

                var urlApi = globalConfiguration.Configuration<string>("NotificacionesAPI");

                await this.FormatMessageAsync(notificacionViewModel)
                    .ConfigureAwait(false);

                if (notificacionViewModel == null)
                {
                    return false;
                }

                notificacionViewModel.Item = null;

                await Task.Run(
                        async () => await HttpRequestFactory.PostAsync(urlApi, notificacionViewModel)
                                        .ConfigureAwait(false))
                    .ConfigureAwait(true);

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private async Task<string> FormatCandidatoAlta(
            NotificacionViewModel notificacionViewModel,
            string notificacionCorreoNotificacion)
        {
            var msg = string.Empty;
            if (!string.IsNullOrEmpty(notificacionCorreoNotificacion))
            {
                msg = notificacionCorreoNotificacion;

                var item = notificacionViewModel.Item;
                Candidato candidato = item.candidato;

                if (candidato != null)
                {
                    msg = msg.Replace("{{Nombre}}", candidato.ToString());
                    msg = msg.Replace("{{Fecha}}", candidato.Created.ToString("dd/MMMM/yyyy"));
                    msg = msg.Replace(
                        "{{Url}}",
                        $"{this.configuration.Configuration<string>("UrlMiRHAplicacion")}mirh");
                }
            }

            // Limpiar Item.
            notificacionViewModel.Item = null;

            return msg;
        }

        private async Task<string> FormatCandidatoCargaInicialInformacion(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;
            var item = notificacionViewModel.Item;
            Candidato candidato = item;
            var date = DateTime.Now;
            msg = notificacion;
            msg = msg.Replace("{{Fecha}}", date.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{FechaNotificacion}}", date.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{NombreCandidato}}", $"{candidato}");
            var urlCandidatoAplicacion = this.configuration.Configuration<string>("UrlCandidatoAplicacion");

            msg = msg.Replace("{{NombreCandidato}}", candidato.ToString());
            msg = msg.Replace("{{FechaNotificacion}}", candidato.Created.ToString("dd/MMMM/yyyy"));
            msg = msg.Replace("{{Url}}", $"{urlCandidatoAplicacion}/auth/login");
            return msg;
        }

        private async Task<string> FormatCandidatoFechaIngreso(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            Candidato candidato = item.candidato;
            RequisicionDetalle requisicion = item.requisicion;
            var date = DateTime.Now;

            Localidad localidad = null;

            if (requisicion.Requisicion.LocalidadId != null)
            {
                localidad = await this.localidadRepository.GetByIdAsync(requisicion.Requisicion.LocalidadId.Value);
            }
            else
            {
                localidad = new Localidad { Descripcion = requisicion.Requisicion.OtraLocalidad };
            }

            var userNameGeneralistaRH = this.configuration.Configuration<string>("UserGeneralistaRH");
            var generalistaRH = await this.userService.GetUserByUserNameAsync(userNameGeneralistaRH);

            msg = notificacion;

            msg = msg.Replace("{{FechaAlta}}", requisicion.FechaIngreso.Value.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{Localidad}}", localidad?.Descripcion);
            msg = msg.Replace("{{DireccionLocalidad}}", localidad?.Descripcion);
            msg = msg.Replace("{{GeneralistaRH}}", generalistaRH.ToString());
            msg = msg.Replace("{{FechaNotificacion}}", date.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{NombreCandidato}}", $"{candidato}");
            var urlCandidatoAplicacion = this.configuration.Configuration<string>("UrlCandidatoAplicacion");
            msg = msg.Replace("{{NombreCandidato}}", candidato.ToString());
            msg = msg.Replace("{{FechaNotificacion}}", candidato.Created.ToString("dd/MMMM/yyyy"));
            msg = msg.Replace("{{Url}}", $"{urlCandidatoAplicacion}/auth/login");
            return msg;
        }

        private async Task<string> FormatEnviarOferta(NotificacionViewModel notificacionViewModel, string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            Candidato candidato = item.candidato;

            msg = notificacion;
            msg = msg.Replace("{{FechaNotificacion}}", DateTime.Now.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{NombreCandidato}}", candidato.ToString() ?? string.Empty);
            return msg;
        }

        private string FormatInvitarCandidato(NotificacionViewModel notificacionViewModel, string notificacion)
        {
            var msg = string.Empty;
            var urlCandidatoAplicacion = this.configuration.Configuration<string>("UrlCandidatoAplicacion");
            var password = this.configuration.Configuration<string>("PwdNuevoCandidato");
            if (!string.IsNullOrEmpty(notificacion))
            {
                msg = notificacion;
                var candidato = (Candidato)notificacionViewModel.Item;
                if (candidato != null)
                {
                    msg = msg.Replace("{{NombreCandidato}}", candidato.ToString());
                    msg = msg.Replace("{{FechaNotificacion}}", DateTime.Now.ToString("dd/MMMM/yyyy"));
                    msg = msg.Replace("{{Url}}", $"{urlCandidatoAplicacion}/auth/login");
                    msg = msg.Replace("{{Email}}", candidato.CandidatoUser.UserName);
                    msg = msg.Replace("{{Password}}", password);
                }
            }

            notificacionViewModel.Item = null;

            return msg;
        }

        private async Task FormatMessageAsync(NotificacionViewModel notificacionViewModel)
        {
            var notificacionCorreo = await this.notificacionCorreosRepository.Single(
                                             new NotificacionCorreosSpecification(notificacionViewModel.TipoEvento))
                                         .ConfigureAwait(false);

            notificacionViewModel.Subject = notificacionCorreo.Asunto;

            var msg = string.Empty;
            var notificacion = notificacionCorreo?.Notificacion;

            switch (notificacionViewModel.TipoEvento)
            {
                case ETipoEvento.AltaCandidato:
                    msg = await this.FormatResetearContrasenia(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.InvitarCandidato:
                    msg = this.FormatInvitarCandidato(notificacionViewModel, notificacion);
                    break;
                case ETipoEvento.SolicitarAutorizacion:
                    msg = await this.FormatSolicitarAutorizacion(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.NotificarAceptacion:
                    msg = await this.FormatSolicitudAutorizacion(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.NotificarRechazo:
                    msg = await this.FormatSolicitudRechazada(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.NotificarCancelacion:
                    msg = await this.FormatSolicitudCancelada(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.NotificarAsignacion:
                    msg = await this.FormatSolicitudAsignacion(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.PublicacionInterna:
                    msg = await this.FormatSolicitudPublicacionInterna(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.SolicitudDocumentosInicial:
                    msg = await this.FormatSolicitudDocumentosInicial(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.NotificacionTernaEntrevista:
                    msg = await this.FormatSolicitudRySEntrevista(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.NotificacionRySEntrevista:
                    msg = await this.FormatSolicitudRySEntrevista(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.AlertaSeguimientoTernas:
                    msg = await this.FormatSolicitudSeguimientoTernas(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.SeleccionCandidato:
                    msg = await this.FormatSolicitudSeleccionCandidato(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.SolicitudDocumentosFinal:
                    msg = await this.FormatSolicitudDocumentosFinal(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.PropuestaProfecionalEconomica:
                    msg = await this.FormatSolicitudPropuestaProfecionalEconomica(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.SolicitudAlta:
                    msg = await this.FormatSolicitudAlta(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.NotificacionFirmaPropuesta:
                    msg = await this.FormatSolicitudFirmaPropuesta(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.ConfirmacionAlta:
                    msg = await this.FormatSolicitudConfirmacionAlta(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.CorreoCandidato:
                    msg = await this.FormatSolicitudCorreoCandidato(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;

                case ETipoEvento.ResetearContrasenia:
                    msg = await this.FormatResetearContrasenia(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;

                case ETipoEvento.NotificarCandidatoCargaComplementariaInformacion:
                    msg = await this.FormatCandidatoCargaInicialInformacion(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.NotificarCandidatoCargaInicialInformacion:
                    break;
                case ETipoEvento.NotificarRHCargaInicialInformacion:
                    break;
                case ETipoEvento.NotificarRHCargaComplementariaInformacion:
                    break;
                case ETipoEvento.NotificarEntrevista:
                    break;
                case ETipoEvento.NotificarEntrevistaRS:
                    break;
                case ETipoEvento.NotificarSeleccionCandidato:
                    msg = await this.FormatNotificarSeleccionCandidato(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.EnviarOferta:
                    msg = await this.FormatEnviarOferta(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.NotificarAltaColaborador:
                    msg = await this.FormatNotificarAltaColaborador(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.NotificarConfirmacionAlta:
                    msg = await this.FormatNotificarAltaColaborador(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.CancelacionSolicitudLimiteTernas:
                    msg = await this.FormatSolicitudSeguimientoTernas(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.NotificacionCandidatoEntrevista:
                    msg = await this.FormatSolicitudRySEntrevista(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.NotificarCandidatoFechaIngreso:
                    msg = await this.FormatCandidatoFechaIngreso(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.NotificarEntrevistaEntrevistador:
                    msg = await this.FormatSolicitudRySEntrevista(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.NotificarCandidatoCargaExpediente:
                    break;
                case ETipoEvento.NotificarRHCargaExpediente:
                    break;
                case ETipoEvento.NotificarCandidatoCorrecionExpediente:
                    break;
                case ETipoEvento.NotificarCandidatoAlta:
                    msg = await this.FormatCandidatoAlta(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.NotificarCargaPropuesta:
                    msg = await this.FormatCargaPropuesta(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                case ETipoEvento.NotificarPropuestaAceptada:
                    msg = await this.FormatPropuestaAceptadaAsync(notificacionViewModel, notificacion)
                              .ConfigureAwait(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            msg = msg.Replace("{{NombreNotificacion}}", notificacionCorreo.Nombre);
            msg = msg.Replace("{{DescripcionNotificacion}}", notificacionCorreo.Descripcion);

            notificacionViewModel.Body = msg;
        }

        private async Task<string> FormatPropuestaAceptadaAsync(NotificacionViewModel notificacionViewModel, string notificacion)
        {
            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            Requisicion requisicion = item.requisicion;
            Candidato candidato = item.candidato;
            var fecha = DateTime.Now;
            List<User> user = item.user;

            var pretencionEconomica = candidato?.CandidatoDetalle?.PretencionEconomica ?? 0;

            requisicion = await this.requisicionRepository.Single(new RequisicionSpecification(requisicion.Id))
                              .ConfigureAwait(false);

            var userRequeridor = await this.userService.GetUserByUserNameAsync(requisicion.UserRequeridor)
                                     .ConfigureAwait(false);

            msg = notificacion;

            msg = msg.Replace("{{Nombre}}", $"{string.Join(';', user.Select(u => u.ToString()))}");
            msg = msg.Replace("{{Fecha}}", fecha.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{IdAlta}}", $"A{requisicion.Id}");
            msg = msg.Replace("{{MotivoIngreso}}", $"{requisicion.MotivoIngreso.Descripcion}");
            msg = msg.Replace("{{NombreUsuarioSolicitante}}", $"{userRequeridor?.ToString()}");
            msg = msg.Replace("{{NombreCandidato}}", candidato.ToString() ?? string.Empty);
            msg = msg.Replace("{{Puesto}}", $"{requisicion?.PuestoSolicitado?.Nombre}");
            msg = msg.Replace(
                "{{Url}}",
                $"{this.configuration.Configuration<string>("UrlMiRHAplicacion")}mirh/requestplace/{requisicion.Id}");
            notificacionViewModel.Subject =
                notificacionViewModel.Subject.Replace("{{idSolicitud}}", $"A{requisicion.Id}");

            // Limpiar Item.
            notificacionViewModel.Item = null;

            return msg;
        }

        private async Task<string> FormatCargaPropuesta(NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            Requisicion requisicion = item.requisicion;
            Candidato candidato = item.candidato;
            var fecha = DateTime.Now;
            List<User> user = item.user;

            var pretencionEconomica = candidato?.CandidatoDetalle?.PretencionEconomica ?? 0;

            requisicion = await this.requisicionRepository.Single(new RequisicionSpecification(requisicion.Id))
                              .ConfigureAwait(false);

            var userRequeridor = await this.userService.GetUserByUserNameAsync(requisicion.UserRequeridor)
                                     .ConfigureAwait(false);

            msg = notificacion;

            msg = msg.Replace("{{Nombre}}", $"{string.Join(';', user.Select(u => u.ToString()))}");
            msg = msg.Replace("{{Fecha}}", fecha.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{IdAlta}}", $"A{requisicion.Id}");
            msg = msg.Replace("{{MotivoIngreso}}", $"{requisicion.MotivoIngreso.Descripcion}");
            msg = msg.Replace("{{NombreUsuarioSolicitante}}", $"{userRequeridor?.ToString()}");

            msg = msg.Replace("{{NombreCandidato}}", candidato.ToString() ?? string.Empty);
            msg = msg.Replace("{{EmpresaAnterior}}", $"{candidato?.CandidatoDetalle?.UltimoTrabajo?.Empresa}");
            msg = msg.Replace("{{Puesto}}", $"{requisicion?.PuestoSolicitado?.Nombre}");
            msg = msg.Replace("{{Salario}}", $"{requisicion?.TabuladorSalario}");
            msg = msg.Replace("{{PretencionEconomica}}", $"{pretencionEconomica:C}");
            msg = msg.Replace(
                "{{Url}}",
                $"{this.configuration.Configuration<string>("UrlMiRHAplicacion")}mirh/requestplace/{requisicion.Id}");
            notificacionViewModel.Subject =
                notificacionViewModel.Subject.Replace("{{idSolicitud}}", $"A{requisicion.Id}");

            // Limpiar Item.
            notificacionViewModel.Item = null;

            return msg;
        }

        private async Task<string> FormatNotificarAltaColaborador(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            Requisicion requisicion = item.requisicion;
            Candidato candidato = item.candidato;
            var fecha = DateTime.Now;
            List<User> user = item.user;
            List<string> actividades = item.actividades;

            var pretencionEconomica = candidato?.CandidatoDetalle?.PretencionEconomica ?? 0;

            requisicion = await this.requisicionRepository.Single(new RequisicionSpecification(requisicion.Id))
                              .ConfigureAwait(false);

            var userRequeridor = await this.userService.GetUserByUserNameAsync(requisicion.UserRequeridor)
                                     .ConfigureAwait(false);

            User jefeDirecto = null;
            if (!string.IsNullOrEmpty(userRequeridor.JefeUserName))
            {
                jefeDirecto = await this.userService.GetUserByUserNameAsync(userRequeridor.JefeUserName)
                                  .ConfigureAwait(false);
            }

            msg = notificacion;

            msg = msg.Replace("{{Nombre}}", $"{string.Join(';', user.Select(u => u.ToString()))}");
            msg = msg.Replace("{{Fecha}}", fecha.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{IdAlta}}", $"A{requisicion.Id}");
            msg = msg.Replace("{{MotivoIngreso}}", $"{requisicion.MotivoIngreso.Descripcion}");

            msg = msg.Replace("{{MotivoIngreso}}", $"{requisicion.MotivoIngreso.Descripcion}");

            msg = msg.Replace("{{Departamento}}", $"{requisicion.Departamento}");
            msg = msg.Replace("{{JefeDirecto}}", $"{userRequeridor}");
            msg = msg.Replace("{{TituloVacante}}", $"{requisicion?.PuestoSolicitado?.Nombre}");
            msg = msg.Replace("{{NivelOrganizacional}}", $"{requisicion.NivelOrganizacional}");

            msg = msg.Replace("{{NombreCandidato}}", candidato.ToString() ?? string.Empty);
            msg = msg.Replace("{{NombreUsuarioSolicitante}}", userRequeridor.ToString() ?? string.Empty);
            msg = msg.Replace("{{FechaNotificacion}}", fecha.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{idRequisicion}}", requisicion.Id.ToString());
            msg = msg.Replace("{{MotivoAlta}}", requisicion?.MotivoIngreso?.Descripcion ?? string.Empty);
            msg = msg.Replace("{{Puesto}}", requisicion.PuestoSolicitado.Nombre ?? string.Empty);
            msg = msg.Replace("{{EmpresaAnterior}}", $"{candidato?.CandidatoDetalle?.UltimoTrabajo?.Empresa}");
            msg = msg.Replace("{{Puesto}}", $"{requisicion?.PuestoSolicitado?.Descripcion}");
            msg = msg.Replace("{{Salario}}", $"{requisicion?.TabuladorSalario}");
            msg = msg.Replace("{{PretencionEconomica}}", $"{pretencionEconomica:C}");
            msg = msg.Replace(
                "{{Actividades}}",
                $"<ul>{string.Join(string.Empty, actividades.Select(a => $"<li>{a}</li>").ToList())}</ul>");
            msg = msg.Replace(
                "{{Terna}}",
                $"{requisicion.RequisicionDetalle.Ternas.FirstOrDefault(t => t.TernaCandidato.Any(tc => tc.CandidatoId == candidato.Id))}");
            msg = msg.Replace(
                "{{Url}}",
                $"{this.configuration.Configuration<string>("UrlMiRHAplicacion")}mirh/requestplace/{requisicion.Id}");
            notificacionViewModel.Subject =
                notificacionViewModel.Subject.Replace("{{idSolicitud}}", $"A{requisicion.Id}");

            return msg;
        }

        private async Task<string> FormatNotificarAsignacion(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            DateTime date = item.fecha;
            User user = item.user;
            int idRequisicion = item.idRequisicion;
            var requisicion = await this.requisicionRepository.Single(new RequisicionSpecification(idRequisicion))
                                  .ConfigureAwait(false);

            var jefeDirecto = await this.userService.GetUserByUserNameAsync(requisicion.UserRequeridor)
                                  .ConfigureAwait(false);

            msg = notificacion;
            msg = msg.Replace("{{Nombre}}", user.ToString());
            msg = msg.Replace("{{Fecha}}", date.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{Departamento}}", requisicion?.Departamento ?? string.Empty);

            msg = msg.Replace("{{JefeDirecto}}", jefeDirecto.ToString());
            msg = msg.Replace("{{TituloVacante}}", requisicion?.PuestoSolicitado?.Nombre ?? string.Empty);

            return msg;
        }

        private async Task<string> FormatNotificarCandidatoCargaInicialInformacion(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;

            string email = item.Email.ToString();
            string code = item.Code.ToString();
            var candidato = await this.candidatoRepository.ListAsync(new CandidatoSpecification(email))
                                .ConfigureAwait(false);

            if (candidato == null
                || string.IsNullOrEmpty(code))
            {
                return msg;
            }

            msg = notificacion;
            msg = msg.Replace("{{Email}}", email);
            msg = msg.Replace("{{Code}}", code);

            return msg;
        }

        private async Task<string> FormatNotificarConfirmacionAlta(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            Requisicion requisicion = item.requisicion;
            Candidato candidato = item.candidato;

            var destinatario = this.configuration.Configuration<string>("UserCoordinadorRS");
            var userDestinatario = await this.userService.GetUserByUserNameAsync(destinatario)
                                       .ConfigureAwait(false);
            var userRequeridor = await this.userService.GetUserByUserNameAsync(requisicion.UserRequeridor)
                                     .ConfigureAwait(false);

            msg = notificacion;
            msg = msg.Replace("{{NombreColaborador}}", userDestinatario.ToString() ?? string.Empty);
            msg = msg.Replace("{{ColaboradorSolicitante}}", userRequeridor.ToString() ?? string.Empty);
            msg = msg.Replace("{{FechaNotificacion}}", DateTime.Now.ToString("dd/MM/yyyy"));

            msg = msg.Replace("{{idRequisicion}}", requisicion.Id.ToString());
            msg = msg.Replace("{{MotivoAlta}}", requisicion?.MotivoIngreso?.Descripcion ?? string.Empty);

            msg = msg.Replace("{{Puesto}}", requisicion.PuestoSolicitado.Nombre ?? string.Empty);

            return msg;
        }

        private async Task<string> FormatNotificarEntrevista(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            Entrevista entrevista = item.entrevista;
            Requisicion requisicion = item.requisicion;
            User entrevistador = item.entrevistador;

            entrevista = await this.entrevistaRepository
                             .Single(new EntrevistaSpecification(requisicion.Id, entrevista.Id))
                             .ConfigureAwait(false);

            msg = notificacion;
            msg = msg.Replace("{{NombreColaborador}}", entrevistador.ToString() ?? string.Empty);
            msg = msg.Replace("{{FechaNotificacion}}", DateTime.Now.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{FechaEntrevista}}", entrevista.FechaInicioEntrevista.Value.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{idRequisicion}}", requisicion.Id.ToString());
            msg = msg.Replace("{{MotivoAlta}}", requisicion?.MotivoIngreso?.Descripcion ?? string.Empty);
            msg = msg.Replace("{{Candidato}}", entrevista.Candidato.ToString() ?? string.Empty);
            msg = msg.Replace(
                "{{EmpresaAnterior}}",
                entrevista.Candidato.CandidatoDetalle.UltimoTrabajo.Empresa ?? string.Empty);
            msg = msg.Replace("{{Puesto}}", requisicion.PuestoSolicitado.Nombre ?? string.Empty);
            msg = msg.Replace(
                "{{PretencionesEconomicas}}",
                entrevista?.Candidato?.CandidatoDetalle?.PretencionEconomica?.ToString("C") ?? "$ 0.00");

            return msg;
        }

        private async Task<string> FormatNotificarEntrevistaRS(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            Entrevista entrevista = item.entrevista;
            Requisicion requisicion = item.requisicion;
            var destinatario = this.configuration.Configuration<string>("UserCoordinadorRS");
            var userDestinatario = await this.userService.GetUserByUserNameAsync(destinatario)
                                       .ConfigureAwait(false);
            var userRequeridor = await this.userService.GetUserByUserNameAsync(requisicion.UserRequeridor)
                                     .ConfigureAwait(false);

            entrevista = await this.entrevistaRepository
                             .Single(new EntrevistaSpecification(requisicion.Id, entrevista.Id))
                             .ConfigureAwait(false);

            msg = notificacion;
            msg = msg.Replace("{{NombreColaborador}}", userDestinatario.ToString() ?? string.Empty);
            msg = msg.Replace("{{ColaboradorSolicitante}}", userRequeridor.ToString() ?? string.Empty);
            msg = msg.Replace("{{FechaNotificacion}}", DateTime.Now.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{FechaEntrevista}}", entrevista.FechaInicioEntrevista.Value.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{TipoEntrevista}}", entrevista.TipoEntrevista.GetDescription());
            msg = msg.Replace("{{idRequisicion}}", requisicion.Id.ToString());
            msg = msg.Replace("{{MotivoAlta}}", requisicion?.MotivoIngreso?.Descripcion ?? string.Empty);
            msg = msg.Replace("{{Candidato}}", entrevista.Candidato.ToString() ?? string.Empty);
            msg = msg.Replace(
                "{{EmpresaAnterior}}",
                entrevista.Candidato.CandidatoDetalle.UltimoTrabajo.Empresa ?? string.Empty);
            msg = msg.Replace("{{Puesto}}", requisicion.PuestoSolicitado.Nombre ?? string.Empty);
            msg = msg.Replace(
                "{{PretencionesEconomicas}}",
                entrevista?.Candidato?.CandidatoDetalle?.PretencionEconomica?.ToString("C") ?? "$ 0.00");

            return msg;
        }

        private async Task<string> FormatNotificarSeleccionCandidato(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            var idRequisicion = item.idRequisicion;
            Candidato candidato = item.candidato;
            DateTime fecha = item.fecha;
            User user = item.user;
            var pretencionEconomica = candidato?.CandidatoDetalle?.PretencionEconomica ?? 0;

            var requisicion = await this.requisicionRepository.Single(new RequisicionSpecification(idRequisicion))
                                  .ConfigureAwait(false);

            var userRequeridor = await this.userService.GetUserByUserNameAsync(requisicion.UserRequeridor)
                                     .ConfigureAwait(false);

            msg = notificacion;

            msg = msg.Replace("{{Nombre}}", $"{user}");
            msg = msg.Replace("{{Fecha}}", fecha.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{IdAlta}}", $"A{requisicion.Id}");
            msg = msg.Replace("{{MotivoIngreso}}", $"{requisicion.MotivoIngreso.Descripcion}");

            msg = msg.Replace("{{NombreColaborador}}", user.ToString() ?? string.Empty);
            msg = msg.Replace("{{NombreCandidato}}", candidato.ToString() ?? string.Empty);
            msg = msg.Replace("{{NombreUsuarioSolicitante}}", userRequeridor.ToString() ?? string.Empty);
            msg = msg.Replace("{{FechaNotificacion}}", fecha.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{idRequisicion}}", requisicion.Id.ToString());
            msg = msg.Replace("{{MotivoAlta}}", requisicion?.MotivoIngreso?.Descripcion ?? string.Empty);
            msg = msg.Replace("{{Puesto}}", requisicion.PuestoSolicitado.Nombre ?? string.Empty);
            msg = msg.Replace("{{EmpresaAnterior}}", $"{candidato?.CandidatoDetalle?.UltimoTrabajo?.Empresa}");
            msg = msg.Replace("{{Puesto}}", $"{requisicion?.PuestoSolicitado?.Descripcion}");
            msg = msg.Replace("{{Salario}}", $"{requisicion?.TabuladorSalario}");
            msg = msg.Replace("{{PretencionEconomica}}", $"{pretencionEconomica:C}");
            msg = msg.Replace(
                "{{Terna}}",
                $"{requisicion.RequisicionDetalle.Ternas.FirstOrDefault(t => t.TernaCandidato.Any(tc => tc.CandidatoId == candidato.Id))}");
            msg = msg.Replace(
                "{{Url}}",
                $"{this.configuration.Configuration<string>("UrlMiRHAplicacion")}mirh/requestplace/{requisicion.Id}");
            notificacionViewModel.Subject =
                notificacionViewModel.Subject.Replace("{{idSolicitud}}", $"A{requisicion.Id}");

            return msg;
        }

        private async Task<string> FormatResetearContrasenia(
            NotificacionViewModel notificacionViewModel,
            string notificacionCorreoNotificacion)
        {
            if (string.IsNullOrEmpty(notificacionCorreoNotificacion))
            {
                return null;
            }

            var msg = string.Empty;

            ResetContrasenia item = notificacionViewModel.Item;
            var email = item.Email;
            var code = item.Code;
            var candidato = await this.candidatoRepository.ListAsync(new CandidatoSpecification(email))
                                .ConfigureAwait(false);
            var urlCandidato = this.configuration.Configuration<string>("UrlCandidatoAplicacion");

            if (candidato == null
                || string.IsNullOrEmpty(code))
            {
                return msg;
            }

            msg = notificacionCorreoNotificacion;
            msg = msg?.Replace("{{Email}}", email);
            msg = msg?.Replace("{{Nombre}}", item.Nombre);
            msg = msg?.Replace("{{Fecha}}", DateTime.Now.ToString("dd/MM/yyyy"));
            msg = msg?.Replace("{{Url}}", urlCandidato);
            msg = msg?.Replace("{{Code}}", code);

            return msg;
        }

        private async Task<string> FormatRHCargaComplementariaInformacion(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;

            string email = item.Email.ToString();
            string code = item.Code.ToString();
            var candidato = await this.candidatoRepository.ListAsync(new CandidatoSpecification(email))
                                .ConfigureAwait(false);

            if (candidato == null
                || string.IsNullOrEmpty(code))
            {
                return msg;
            }

            msg = notificacion;
            msg = msg.Replace("{{Email}}", email);
            msg = msg.Replace("{{Code}}", code);

            return msg;
        }

        private async Task<string> FormatRHCargaInicialInformacion(
            NotificacionViewModel notificacionViewModel,
            string notificacionCorreoNotificacion)
        {
            if (string.IsNullOrEmpty(notificacionCorreoNotificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;

            string email = item.CandidatoUser.ToString();
            var candidato = await this.candidatoRepository.ListAsync(new CandidatoSpecification(email))
                                .ConfigureAwait(false);

            msg = notificacionCorreoNotificacion;
            msg = msg.Replace("{{Email}}", email);

            return msg;
        }

        private async Task<string> FormatSolicitarAutorizacion(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            DateTime date = item.fecha;
            User user = item.user;
            int idRequisicion = item.idRequisicion;
            var requisicion = await this.requisicionRepository.Single(new RequisicionSpecification(idRequisicion))
                                  .ConfigureAwait(false);

            var jefeDirecto = await this.userService.GetUserByUserNameAsync(requisicion.UserRequeridor)
                                  .ConfigureAwait(false);

            var tabulador = requisicion?.TabuladorSalario;

            msg = notificacion;
            msg = msg.Replace("{{Nombre}}", user.ToString());
            msg = msg.Replace("{{Fecha}}", date.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{IdAlta}}", $"A{requisicion.Id}");
            msg = msg.Replace("{{MotivoIngreso}}", $"{requisicion.MotivoIngreso.Descripcion}");
            msg = msg.Replace("{{NombreUsuarioSolicitante}}", $"{jefeDirecto}");
            msg = msg.Replace("{{Departamento}}", $"{requisicion.Departamento}");
            msg = msg.Replace("{{JefeDirecto}}", $"{jefeDirecto}");
            msg = msg.Replace("{{TituloVacante}}", $"{requisicion?.PuestoSolicitado?.Nombre}");
            msg = msg.Replace("{{NivelOrganizacional}}", $"{requisicion.NivelOrganizacional}");

            msg = msg.Replace(
                "{{NivelTabulador}}",
                tabulador != null
                    ? $"{tabulador.Minimo:C} - {tabulador.Maximo:C}"
                    : string.Empty);

            msg = msg.Replace(
                "{{Url}}",
                $"{this.configuration.Configuration<string>("UrlMiRHAplicacion")}mirh/requestplace/{requisicion.Id}");

            notificacionViewModel.Subject =
                notificacionViewModel.Subject.Replace("{{idSolicitud}}", $"A{requisicion.Id}");

            return msg;
        }

        private async Task<string> FormatSolicitudAlta(NotificacionViewModel notificacionViewModel, string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            DateTime date = item.fecha;
            User user = item.user;
            int idRequisicion = item.idRequisicion;
            var requisicion = await this.requisicionRepository.Single(new RequisicionSpecification(idRequisicion))
                                  .ConfigureAwait(false);

            msg = notificacion;
            msg = msg.Replace("{{Nombre}}", user.ToString());
            msg = msg.Replace("{{Fecha}}", date.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{IdAlta}}", $"A{requisicion.Id}");
            msg = msg.Replace("{{MotivoIngreso}}", $"{requisicion.MotivoIngreso.Descripcion}");
            msg = msg.Replace("{{NombreUsuarioSolicitante}}", $"{requisicion.UserRequeridor}");

            msg = msg.Replace(
                "{{Url}}",
                $"{this.configuration.Configuration<string>("UrlMiRHAplicacion")}mirh/requestplace/{requisicion.Id}");

            return msg;
        }

        private async Task<string> FormatSolicitudAsignacion(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            DateTime date = item.fecha;
            User user = item.user;
            var idRequisicion = item.idRequisicion;

            // var comentario = item.comentario ?? string.Empty;
            var requisicion = await this.requisicionRepository.Single(new RequisicionSpecification(idRequisicion))
                                  .ConfigureAwait(false);
            var jefeDirecto = await this.userService.GetUserByUserNameAsync(requisicion.UserRequeridor)
                                  .ConfigureAwait(false);
            var tabulador = requisicion?.TabuladorSalario;

            msg = notificacion;
            msg = msg.Replace("{{Nombre}}", user.ToString());
            msg = msg.Replace("{{Fecha}}", date.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{IdAlta}}", $"A{requisicion.Id}");
            msg = msg.Replace("{{MotivoIngreso}}", $"{requisicion.MotivoIngreso.Descripcion}");
            msg = msg.Replace("{{NombreUsuarioSolicitante}}", $"{jefeDirecto}");
            msg = msg.Replace("{{Departamento}}", $"{requisicion.Departamento}");
            msg = msg.Replace("{{JefeDirecto}}", $"{jefeDirecto}");
            msg = msg.Replace("{{TituloVacante}}", $"{requisicion?.PuestoSolicitado?.Nombre}");
            msg = msg.Replace("{{NivelOrganizacional}}", $"{requisicion.NivelOrganizacional}");

            // msg = msg.Replace("{{Comentario}}", $"{comentario}");
            msg = msg.Replace(
                "{{NivelTabulador}}",
                tabulador != null
                    ? $"{tabulador.Minimo:C} - {tabulador.Maximo:C}"
                    : string.Empty);

            msg = msg.Replace(
                "{{Url}}",
                $"{this.configuration.Configuration<string>("UrlMiRHAplicacion")}mirh/requestplace/{requisicion.Id}");

            notificacionViewModel.Subject =
                notificacionViewModel.Subject.Replace("{{idSolicitud}}", $"A{requisicion.Id}");

            return msg;
        }

        private async Task<string> FormatSolicitudAutorizacion(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            DateTime date = item.fecha;
            User user = item.user;
            var idRequisicion = item.idRequisicion;
            var comentario = item.comentario ?? string.Empty;

            var requisicion = await this.requisicionRepository.Single(new RequisicionSpecification(idRequisicion))
                                  .ConfigureAwait(false);
            var jefeDirecto = await this.userService.GetUserByUserNameAsync(requisicion.UserRequeridor)
                                  .ConfigureAwait(false);
            var tabulador = requisicion?.TabuladorSalario;

            msg = notificacion;
            msg = msg.Replace("{{Nombre}}", user.ToString());
            msg = msg.Replace("{{Fecha}}", date.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{IdAlta}}", $"A{requisicion.Id}");
            msg = msg.Replace("{{MotivoIngreso}}", $"{requisicion.MotivoIngreso.Descripcion}");
            msg = msg.Replace("{{NombreUsuarioSolicitante}}", $"{jefeDirecto}");
            msg = msg.Replace("{{Departamento}}", $"{requisicion.Departamento}");
            msg = msg.Replace("{{JefeDirecto}}", $"{jefeDirecto}");
            msg = msg.Replace("{{TituloVacante}}", $"{requisicion?.PuestoSolicitado?.Nombre}");
            msg = msg.Replace("{{NivelOrganizacional}}", $"{requisicion.NivelOrganizacional}");
            msg = msg.Replace("{{Comentario}}", $"{comentario}");

            msg = msg.Replace(
                "{{NivelTabulador}}",
                tabulador != null
                    ? $"{tabulador.Minimo:C} - {tabulador.Maximo:C}"
                    : string.Empty);

            msg = msg.Replace(
                "{{Url}}",
                $"{this.configuration.Configuration<string>("UrlMiRHAplicacion")}mirh/requestplace/{requisicion.Id}");

            notificacionViewModel.Subject =
                notificacionViewModel.Subject.Replace("{{idSolicitud}}", $"A{requisicion.Id}");

            return msg;
        }

        private async Task<string> FormatSolicitudCancelada(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            DateTime date = item.fecha;
            User user = item.user;
            var idRequisicion = item.idRequisicion;
            var comentario = item.comentario ?? string.Empty;

            var requisicion = await this.requisicionRepository.Single(new RequisicionSpecification(idRequisicion))
                                  .ConfigureAwait(false);
            var jefeDirecto = await this.userService.GetUserByUserNameAsync(requisicion.UserRequeridor)
                                  .ConfigureAwait(false);
            var tabulador = requisicion?.TabuladorSalario;

            msg = notificacion;
            msg = msg.Replace("{{Nombre}}", $"{jefeDirecto}");
            msg = msg.Replace("{{Fecha}}", date.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{IdAlta}}", $"A{requisicion.Id}");
            msg = msg.Replace("{{MotivoIngreso}}", $"{requisicion.MotivoIngreso.Descripcion}");
            msg = msg.Replace("{{NombreUsuarioSolicitante}}", $"{jefeDirecto}");
            msg = msg.Replace("{{Departamento}}", $"{requisicion.Departamento}");
            msg = msg.Replace("{{JefeDirecto}}", $"{jefeDirecto}");
            msg = msg.Replace("{{TituloVacante}}", $"{requisicion?.PuestoSolicitado?.Nombre}");
            msg = msg.Replace("{{NivelOrganizacional}}", $"{requisicion.NivelOrganizacional}");
            msg = msg.Replace("{{Comentario}}", $"{comentario}");

            msg = msg.Replace(
                "{{NivelTabulador}}",
                tabulador != null
                    ? $"{tabulador.Minimo:C} - {tabulador.Maximo:C}"
                    : string.Empty);

            msg = msg.Replace(
                "{{Url}}",
                $"{this.configuration.Configuration<string>("UrlMiRHAplicacion")}mirh/requestplace/{requisicion.Id}");

            notificacionViewModel.Subject =
                notificacionViewModel.Subject.Replace("{{idSolicitud}}", $"A{requisicion.Id}");

            return msg;
        }

        private async Task<string> FormatSolicitudConfirmacionAlta(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            DateTime date = item.fecha;
            User user = item.user;
            var idRequisicion = item.idRequisicion;

            var requisicion = await this.requisicionRepository.Single(new RequisicionSpecification(idRequisicion))
                                  .ConfigureAwait(false);

            msg = notificacion;
            msg = msg.Replace("{{Nombre}}", user.ToString());
            msg = msg.Replace("{{Fecha}}", date.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{IdAlta}}", $"A{requisicion.Id}");
            msg = msg.Replace("{{MotivoIngreso}}", $"{requisicion.MotivoIngreso.Descripcion}");
            msg = msg.Replace("{{NombreUsuarioSolicitante}}", $"{requisicion.UserRequeridor}");

            msg = msg.Replace("{{NombreCompleto}}", "XXX");
            msg = msg.Replace("{{Departamento}}", "XXX");
            msg = msg.Replace("{{CentroCosto}}", "XXX");
            msg = msg.Replace("{{JefeDirecto}}", "XXX");
            msg = msg.Replace("{{Puesto}}", $"{requisicion.PuestoSolicitado.Nombre}");
            msg = msg.Replace("{{NivelOrganizacional}}", "XXX");

            msg = msg.Replace(
                "{{Url}}",
                $"{this.configuration.Configuration<string>("UrlMiRHAplicacion")}mirh/requestplace/{requisicion.Id}");
            notificacionViewModel.Subject =
                notificacionViewModel.Subject.Replace("{{idSolicitud}}", $"A{requisicion.Id}");
            return msg;
        }

        private async Task<string> FormatSolicitudCorreoCandidato(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            DateTime date = item.fecha;
            User user = item.user;

            msg = notificacion;
            msg = msg.Replace("{{NombreCandidato}}", user.ToString());
            msg = msg.Replace("{{FechaNotificacion}}", date.ToString("dd/MM/yyyy"));

            msg = msg.Replace("{{fechaAlta}}", "XXX");
            msg = msg.Replace("{{Localidad}}", "XXX");
            msg = msg.Replace("{{DireccionLocalidad}}", "XXX");
            msg = msg.Replace("{{GeneralistaRH}}", "XXX");

            msg = msg.Replace("{{Url}}", "XXX");

            return msg;
        }

        private async Task<string> FormatSolicitudDocumentosFinal(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            DateTime date = item.fecha;
            User user = item.user;

            msg = notificacion;
            msg = msg.Replace("{{NombreCandidato}}", user.ToString());
            msg = msg.Replace("{{FechaNotificacion}}", date.ToString("dd/MM/yyyy"));

            msg = msg.Replace("{{Url}}", "XXX");

            return msg;
        }

        private async Task<string> FormatSolicitudDocumentosInicial(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            DateTime date = item.fecha;
            User user = item.user;

            msg = notificacion;
            msg = msg.Replace("{{NombreCandidato}}", user.ToString());
            msg = msg.Replace("{{FechaNotificacion}}", date.ToString("dd/MM/yyyy"));

            msg = msg.Replace("{{Url}}", "XXX");

            return msg;
        }

        private async Task<string> FormatSolicitudFirmaPropuesta(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            DateTime date = item.fecha;
            User user = item.user;

            msg = notificacion;
            msg = msg.Replace("{{NombreCandidato}}", user.ToString());
            msg = msg.Replace("{{FechaNotificacion}}", date.ToString("dd/MM/yyyy"));

            msg = msg.Replace("{{Url}}", "XXX");

            return msg;
        }

        private async Task<string> FormatSolicitudPropuestaProfecionalEconomica(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            DateTime date = item.fecha;
            User user = item.user;

            msg = notificacion;
            msg = msg.Replace("{{NombreCandidato}}", user.ToString());
            msg = msg.Replace("{{FechaNotificacion}}", date.ToString("dd/MM/yyyy"));

            return msg;
        }

        private async Task<string> FormatSolicitudPublicacionInterna(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;

            User user = item.user;
            int idRequisicion = item.idRequisicion;

            var requisicion = await this.requisicionRepository.Single(new RequisicionSpecification(idRequisicion))
                                  .ConfigureAwait(false);
            var jefeDirecto = await this.userService.GetUserByUserNameAsync(requisicion.UserRequeridor)
                                  .ConfigureAwait(false);
            var tabulador = requisicion?.TabuladorSalario;

            msg = notificacion;
            msg = msg.Replace("{{Nombre}}", $"{user}");
            msg = msg.Replace("{{Fecha}}", DateTime.Now.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{IdAlta}}", $"A{requisicion.Id}");
            msg = msg.Replace("{{MotivoIngreso}}", $"{requisicion.MotivoIngreso.Descripcion}");
            msg = msg.Replace("{{NombreUsuarioSolicitante}}", $"{jefeDirecto}");
            msg = msg.Replace("{{Departamento}}", $"{requisicion.Departamento}");
            msg = msg.Replace("{{JefeDirecto}}", $"{jefeDirecto}");
            msg = msg.Replace("{{TituloVacante}}", $"{requisicion?.PuestoSolicitado?.Nombre}");
            msg = msg.Replace("{{NivelOrganizacional}}", $"{requisicion.NivelOrganizacional}");

            // msg = msg.Replace("{{Comentario}}", $"{comentario}");
            msg = msg.Replace(
                "{{NivelTabulador}}",
                tabulador != null
                    ? $"{tabulador.Minimo:C} - {tabulador.Maximo:C}"
                    : string.Empty);

            msg = msg.Replace(
                "{{Url}}",
                $"{this.configuration.Configuration<string>("UrlMiRHAplicacion")}mirh/requestplace/{requisicion.Id}");

            notificacionViewModel.Subject =
                notificacionViewModel.Subject.Replace("{{idSolicitud}}", $"A{requisicion.Id}");

            return msg;
        }

        private async Task<string> FormatSolicitudRechazada(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            DateTime date = item.fecha;
            User user = item.user;
            var idRequisicion = item.idRequisicion;
            var comentario = item.comentario ?? string.Empty;

            var requisicion = await this.requisicionRepository.Single(new RequisicionSpecification(idRequisicion))
                                  .ConfigureAwait(false);
            var jefeDirecto = await this.userService.GetUserByUserNameAsync(requisicion.UserRequeridor)
                                  .ConfigureAwait(false);
            var tabulador = requisicion?.TabuladorSalario;

            msg = notificacion;
            msg = msg.Replace("{{Nombre}}", user.ToString());
            msg = msg.Replace("{{Fecha}}", date.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{IdAlta}}", $"A{requisicion.Id}");
            msg = msg.Replace("{{MotivoIngreso}}", $"{requisicion.MotivoIngreso.Descripcion}");
            msg = msg.Replace("{{NombreUsuarioSolicitante}}", $"{jefeDirecto}");
            msg = msg.Replace("{{Departamento}}", $"{requisicion.Departamento}");
            msg = msg.Replace("{{JefeDirecto}}", $"{jefeDirecto}");
            msg = msg.Replace("{{TituloVacante}}", $"{requisicion?.PuestoSolicitado?.Nombre}");
            msg = msg.Replace("{{NivelOrganizacional}}", $"{requisicion.NivelOrganizacional}");
            msg = msg.Replace("{{Comentario}}", $"{comentario}");

            msg = msg.Replace(
                "{{NivelTabulador}}",
                tabulador != null
                    ? $"{tabulador.Minimo:C} - {tabulador.Maximo:C}"
                    : string.Empty);

            msg = msg.Replace(
                "{{Url}}",
                $"{this.configuration.Configuration<string>("UrlMiRHAplicacion")}mirh/requestplace/{requisicion.Id}");

            notificacionViewModel.Subject =
                notificacionViewModel.Subject.Replace("{{idSolicitud}}", $"A{requisicion.Id}");

            return msg;
        }

        private async Task<string> FormatSolicitudRySEntrevista(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            var date = DateTime.Now;
            User user = item.entrevistador;
            Entrevista entrevista = item.entrevista;
            var fechaEntrevista = entrevista.FechaInicioEntrevista.Value;
            Requisicion requisicion = item.requisicion;
            requisicion = await this.requisicionRepository.Single(new RequisicionSpecification(requisicion.Id))
                              .ConfigureAwait(false);

            var entrevistador = await this.userService.GetUserByUserNameAsync(entrevista.Entrevistador);

            var userSolicitante = await this.userService.GetUserByUserNameAsync(requisicion.UserRequeridor);
            var pretencionEconomica = entrevista?.Candidato?.CandidatoDetalle?.PretencionEconomica ?? 0;

            msg = notificacion;
            msg = msg.Replace("{{Nombre}}", $"{user}");
            msg = msg.Replace("{{Fecha}}", date.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{IdAlta}}", $"A{requisicion.Id}");
            msg = msg.Replace("{{MotivoIngreso}}", $"{requisicion.MotivoIngreso.Descripcion}");
            msg = msg.Replace("{{NombreEntrevista}}", $"{entrevista.TipoEntrevista.GetDescription()}");
            msg = msg.Replace("{{NombreEntrevistador}}", $"{entrevistador}");

            msg = msg.Replace("{{NombreUsuarioSolicitante}}", $"{userSolicitante}");
            msg = msg.Replace("{{FechaNotificacion}}", date.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{FechaEntrevista}}", fechaEntrevista.ToString("dd/MM/yyyy HH:mm"));
            msg = msg.Replace("{{IdAlta}}", $"A{requisicion.Id}");
            msg = msg.Replace("{{MotivoIngreso}}", $"{requisicion?.MotivoIngreso?.Descripcion}");

            msg = msg.Replace("{{NombreCandidato}}", $"{entrevista?.Candidato}");
            msg = msg.Replace(
                "{{EmpresaAnterior}}",
                $"{entrevista?.Candidato?.CandidatoDetalle?.UltimoTrabajo?.Empresa}");
            msg = msg.Replace("{{Puesto}}", $"{entrevista?.Candidato?.CandidatoDetalle?.PuestoSolicitado?.Descripcion}");
            msg = msg.Replace("{{UltimoPuesto}}", $"{entrevista?.Candidato?.CandidatoDetalle?.UltimoTrabajo?.Puesto}");
            msg = msg.Replace("{{Salario}}", $"{entrevista?.Candidato?.CandidatoDetalle?.UltimoSalario}");
            msg = msg.Replace("{{PretencionEconomica}}", $"{pretencionEconomica:C}");
            msg = msg.Replace(
                "{{Terna}}",
                $"{requisicion.RequisicionDetalle.Ternas.FirstOrDefault(t => t.TernaCandidato.Any(tc => tc.CandidatoId == entrevista.CandidatoId)).TipoTerna.GetDescription()}");

            msg = msg.Replace(
                "{{Url}}",
                $"{this.configuration.Configuration<string>("UrlMiRHAplicacion")}mirh/requestplace/{requisicion.Id}");
            notificacionViewModel.Subject =
                notificacionViewModel.Subject.Replace("{{idSolicitud}}", $"A{requisicion.Id}");
            return msg;
        }

        private async Task<string> FormatSolicitudSeguimientoTernas(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            DateTime date = item.fecha;
            List<User> users = item.user;
            int idRequisicion = item.idRequisicion;
            var requisicion = await this.requisicionRepository.Single(new RequisicionSpecification(idRequisicion))
                                  .ConfigureAwait(false);

            var requeridor = await this.userService.GetUserByUserNameAsync(requisicion.UserRequeridor)
                                 .ConfigureAwait(false);

            msg = notificacion;
            msg = msg.Replace("{{Nombre}}", string.Join("; ", users.Select(u => u.ToString())));
            msg = msg.Replace("{{Fecha}}", date.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{IdAlta}}", $"A{requisicion.Id}");
            msg = msg.Replace("{{MotivoIngreso}}", $"{requisicion.MotivoIngreso.Descripcion}");
            msg = msg.Replace("{{NombreUsuarioSolicitante}}", $"{requeridor}");

            msg = msg.Replace(
                "{{Url}}",
                $"{this.configuration.Configuration<string>("UrlMiRHAplicacion")}mirh/requestplace/{requisicion.Id}");
            notificacionViewModel.Subject =
                notificacionViewModel.Subject.Replace("{{idSolicitud}}", $"A{requisicion.Id}");
            return msg;
        }

        private async Task<string> FormatSolicitudSeleccionCandidato(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            DateTime date = item.fecha;
            User user = item.user;
            int idRequisicion = item.idRequisicion;
            var requisicion = await this.requisicionRepository.Single(new RequisicionSpecification(idRequisicion))
                                  .ConfigureAwait(false);

            msg = notificacion;
            msg = msg.Replace("{{Nombre}}", user.ToString());
            msg = msg.Replace("{{Fecha}}", date.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{IdAlta}}", $"A{requisicion.Id}");
            msg = msg.Replace("{{MotivoIngreso}}", $"{requisicion.MotivoIngreso.Descripcion}");
            msg = msg.Replace("{{NombreUsuarioSolicitante}}", $"{requisicion.UserRequeridor}");

            msg = msg.Replace(
                "{{Url}}",
                $"{this.configuration.Configuration<string>("UrlMiRHAplicacion")}mirh/requestplace/{requisicion.Id}");
            notificacionViewModel.Subject =
                notificacionViewModel.Subject.Replace("{{idSolicitud}}", $"A{requisicion.Id}");
            return msg;
        }

        private async Task<string> FormatSolicitudTernaEntrevista(
            NotificacionViewModel notificacionViewModel,
            string notificacion)
        {
            if (string.IsNullOrEmpty(notificacion))
            {
                return null;
            }

            var msg = string.Empty;

            var item = notificacionViewModel.Item;
            var date = DateTime.Now;
            User user = item.entrevistador;
            Entrevista entrevista = item.entrevista;
            var fechaEntrevista = entrevista.FechaInicioEntrevista.Value;
            var requisicion = item.requisicion;
            var requi = await this.requisicionRepository.Single(new RequisicionSpecification(requisicion.Id))
                            .ConfigureAwait(false);
            var userSolicitante = await this.userService.GetUserByUserNameAsync(requi.UserRequeridor);
            var pretencionEconomica = entrevista?.Candidato?.CandidatoDetalle?.PretencionEconomica ?? 0;

            msg = notificacion;
            msg = msg.Replace("{{NombreUsuarioSolicitante}}", $"{userSolicitante}");
            msg = msg.Replace("{{FechaNotificacion}}", date.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{FechaEntrevista}}", fechaEntrevista.ToString("dd/MM/yyyy"));
            msg = msg.Replace("{{IdAlta}}", $"A{requisicion.Id}");
            msg = msg.Replace("{{MotivoIngreso}}", $"{requisicion?.MotivoIngreso?.Descripcion}");

            msg = msg.Replace("{{NombreCandidato}}", $"{entrevista?.Candidato}");
            msg = msg.Replace(
                "{{EmpresaAnterior}}",
                $"{entrevista?.Candidato?.CandidatoDetalle?.UltimoTrabajo?.Empresa}");
            msg = msg.Replace("{{Puesto}}", $"{requi?.PuestoSolicitado?.Descripcion}");
            msg = msg.Replace("{{Salario}}", $"{requi?.TabuladorSalario}");
            msg = msg.Replace("{{PretencionEconomica}}", $"{pretencionEconomica:C}");
            msg = msg.Replace(
                "{{Terna}}",
                $"{requi.RequisicionDetalle.Ternas.FirstOrDefault(t => t.TernaCandidato.Any(tc => tc.CandidatoId == entrevista.CandidatoId))}");
            msg = msg.Replace(
                "{{Url}}",
                $"{this.configuration.Configuration<string>("UrlMiRHAplicacion")}mirh/requestplace/{requisicion.Id}");
            notificacionViewModel.Subject =
                notificacionViewModel.Subject.Replace("{{idSolicitud}}", $"A{requisicion.Id}");
            return msg;
        }
    }

    public class ResetContrasenia
    {
        public string Code { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
    }
}
