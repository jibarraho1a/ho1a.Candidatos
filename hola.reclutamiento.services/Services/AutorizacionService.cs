using AutoMapper;
using ho1a.Api.HttpClient;
using ho1a.applicationCore.Data.Interfaces;
using ho1a.reclutamiento.enums.Notificacion;
using ho1a.reclutamiento.enums.Plazas;
using ho1a.reclutamiento.models.Configuracion;
using ho1a.reclutamiento.models.Plazas;
using ho1a.reclutamiento.models.Seguridad;
using ho1a.reclutamiento.services.Services.Interfaces;
using ho1a.reclutamiento.services.Specifications;
using ho1a.reclutamiento.services.ViewModels.Notificaciones;
using ho1a.reclutamiento.services.ViewModels.Requisicion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services
{
    public class AutorizacionService : IAutorizacionService
    {
        private readonly IGlobalConfiguration<Configuracion> configuracionGlobal;
        private readonly IMapper mapper;
        private readonly INotificarService notificarService;
        private readonly IUserService userService;
        private readonly IAsyncRepository<ValidaRequisicion> validacionRepository;
        public AutorizacionService(
            IMapper mapper,
            IGlobalConfiguration<Configuracion> configuracionGlobal,
            IUserService userService,
            IAsyncRepository<ValidaRequisicion> validacionRepository,
            INotificarService notificarService)
        {
            this.mapper = mapper;
            this.configuracionGlobal = configuracionGlobal;
            this.userService = userService;
            this.validacionRepository = validacionRepository;
            this.notificarService = notificarService;
        }

        public async Task<List<ValidacionesRequisicionViewModel>> AddValidacionAsync(
            int idRequisicion,
            string userNameRequeridor,
            string userNameValidador,
            string motivoIngresoDescripcion,
            ENivelValidacion nivelValidacion)
        {
            var validaciones = await this.GetMatrizAprobacionAsync(
                                   userNameRequeridor,
                                   idRequisicion,
                                   motivoIngresoDescripcion).ConfigureAwait(false);

            var itemsSaved = new List<ValidacionesRequisicionViewModel>();

            if ((bool)!validaciones?.Any(v => v != null && v.NivelValidacion == nivelValidacion))
            {
                validaciones.Add(
                    new ValidaRequisicion
                    {
                        AprobadorUserName = userNameValidador,
                        NivelValidacion = nivelValidacion,
                        RequisicionId = idRequisicion,
                        EstadoValidacion = EEstadoValidacion.Pendiente
                    });

                itemsSaved = await this.AddOrUpdateAsync(idRequisicion, userNameRequeridor, validaciones)
                                 .ConfigureAwait(false);
            }

            var hasActual = itemsSaved.Any(v => v.StateValidation == EEstadoValidacion.Actual);

            if (!hasActual)
            {
                var pendiente = validaciones.Where(v => v.EstadoValidacion == EEstadoValidacion.Pendiente)
                    .OrderBy(v => v.Fecha).First();

                if (pendiente != null)
                {
                    pendiente.EstadoValidacion = EEstadoValidacion.Actual;
                }

                itemsSaved = await this.AddOrUpdateAsync(idRequisicion, userNameRequeridor, validaciones)
                                 .ConfigureAwait(false);
            }

            return itemsSaved;
        }

        public async Task<List<ValidacionesRequisicionViewModel>> AprobacionAsync(
            int idRequisicion,
            string userNameRequeridor,
            string motivoIngresoDescripcion,
            ValidacionesRequisicionViewModel validacion)
        {
            var validaciones = await this.GetMatrizAprobacionAsync(
                                   userNameRequeridor,
                                   idRequisicion,
                                   motivoIngresoDescripcion).ConfigureAwait(false);

            var requisicionToValidate = new RequisicionPlazaViewModel
            {
                UserRequisicion = userNameRequeridor,
                Validaciones = this.mapper.Map<List<ValidacionRequisicionPlaza>>(validaciones),
                ToValidate = validacion
            };

            var matrizAprobacion = await this.ValidarRequisicionAsync(requisicionToValidate).ConfigureAwait(false);

            var result = await this.AlmacenarCambiosAsync(idRequisicion, userNameRequeridor, matrizAprobacion)
                             .ConfigureAwait(false);

            var toValidate = result.FirstOrDefault(v => v.StateValidation == EEstadoValidacion.Actual);

            var validated = validacion;

            switch (validacion.StateValidation)
            {
                case EEstadoValidacion.Cancelado:
                    await this.NotificarAsync(idRequisicion, validated, toValidate, ETipoEvento.NotificarCancelacion)
                        .ConfigureAwait(false);
                    break;
                case EEstadoValidacion.Rechazada:
                    await this.NotificarAsync(idRequisicion, validated, toValidate, ETipoEvento.NotificarRechazo)
                        .ConfigureAwait(false);
                    break;
                case EEstadoValidacion.Aprobada:
                    var tipoEvento = validaciones.Any(v => v.EstadoValidacion == EEstadoValidacion.Rechazada)
                                         ? ETipoEvento.SolicitarAutorizacion
                                         : ETipoEvento.SolicitarAutorizacion;

                    await this.NotificarAsync(idRequisicion, validated, toValidate, tipoEvento).ConfigureAwait(false);
                    break;
            }

            return result;
        }

        public async Task AprobacionTopeSalarialAsync(int idRequisicion, string userName, ValidaRequisicion validacion)
        {
            validacion.EstadoValidacion = EEstadoValidacion.Actual;

            await this.validacionRepository.UpdateAsync(validacion).ConfigureAwait(false);
        }

        public Task<List<ValidaRequisicion>> SetPermisosAsync(
            List<ValidacionesRequisicionViewModel> matrizAprobacion,
            string currentUserName,
            bool isAdmin = false)
        {
            var isCanceled = matrizAprobacion.Any(v => v.StateValidation == EEstadoValidacion.Cancelado);

            var validacionActual = matrizAprobacion.FirstOrDefault(
                v => v.StateValidation == EEstadoValidacion.Actual || v.StateValidation == EEstadoValidacion.Pendiente);

            this.SetPermisos(validacionActual, currentUserName, isAdmin, isCanceled);

            var result = this.mapper.Map<List<ValidaRequisicion>>(matrizAprobacion);
            return Task.FromResult(result.OrderBy(v => v.NivelValidacion).ToList());
        }

        public async Task<List<ValidacionesRequisicionViewModel>> SolicitarAutorizacionAsync(
            int idRequisicion,
            string userName,
            string motivoIngreso)
        {
            var validaciones = await this.GetMatrizAprobacionAsync(userName, idRequisicion, motivoIngreso)
                                   .ConfigureAwait(false);

            var requisicionToValidate = new RequisicionPlazaViewModel
            {
                UserRequisicion = userName,
                Validaciones = this.mapper?.Map<List<ValidacionRequisicionPlaza>>(validaciones),
                ToValidate = null
            };

            var matrizAprobacion = await this.ValidarRequisicionAsync(requisicionToValidate).ConfigureAwait(false);

            var result = await this.AlmacenarCambiosAsync(idRequisicion, userName, matrizAprobacion)
                             .ConfigureAwait(false);

            var toValidate = result.FirstOrDefault(v => v.StateValidation == EEstadoValidacion.Actual);

            if (toValidate != null)
            {
                await this.NotificarAsync(idRequisicion, null, toValidate, ETipoEvento.SolicitarAutorizacion)
                    .ConfigureAwait(false);
            }
            else
            {
                var aceptadas = result.All(v => v.StateValidation == EEstadoValidacion.Aprobada);
                toValidate = result.OrderByDescending(v => v.Date).FirstOrDefault();

                if (aceptadas)
                {
                    await this.NotificarAsync(idRequisicion, null, toValidate, ETipoEvento.NotificarAceptacion)
                        .ConfigureAwait(false);
                }
            }

            return result.OrderBy(x => x.NivelValidacion).ToList();
        }

        private async Task<List<ValidacionesRequisicionViewModel>> AddOrUpdateAsync(
            int idRequisicion,
            string userName,
            List<ValidaRequisicion> validaciones)
        {
            validaciones = this.TopeSalarial(validaciones);

            var saved = await this.validacionRepository.ListAsync(new ValidaRequisicionSpecification(idRequisicion));

            if ((bool)validaciones?.All(v => v.Id != 0)
                && saved.Any()
                && validaciones.Any()
                && saved.Count() == validaciones.Count)
            {
                return await this.UpdateAsync(idRequisicion, userName, validaciones).ConfigureAwait(false);
            }

            await this.UpdateAsync(
                    idRequisicion,
                    userName,
                    validaciones.Where(v => v.Id != 0 && v.EstadoValidacion != EEstadoValidacion.Pendiente).ToList())
                .ConfigureAwait(false);

            foreach (var validaRequisicion in saved.Where(
                v => v.Id != 0 && v.EstadoValidacion == EEstadoValidacion.Pendiente))
            {
                validaRequisicion.Active = false;
                await this.validacionRepository.UpdateAsync(validaRequisicion).ConfigureAwait(false);
            }

            foreach (var validacion in validaciones.Where(
                v => v != null && (v.Id == 0 || v.EstadoValidacion == EEstadoValidacion.Pendiente)))
            {
                validacion.RequisicionId = idRequisicion;
                validacion.CreatedBy = userName;
                validacion.Comentario = null;
                validacion.Id = 0;
                await this.validacionRepository.AddAsync(validacion).ConfigureAwait(false);
            }

            saved = await this.validacionRepository.ListAsync(new ValidaRequisicionSpecification(idRequisicion));

            var actual = validaciones.FirstOrDefault(v => v.EstadoValidacion == EEstadoValidacion.Actual);

            if (actual != null)
            {
                var toEdit = saved.FirstOrDefault(
                    v => v.AprobadorUserName == actual.AprobadorUserName
                         && v.NivelValidacion == actual.NivelValidacion);

                toEdit.EstadoValidacion = EEstadoValidacion.Actual;

                await this.validacionRepository.UpdateAsync(toEdit);
            }

            validaciones = await this.validacionRepository.ListAsync(new ValidaRequisicionSpecification(idRequisicion));

            var result = this.mapper.Map<List<ValidacionesRequisicionViewModel>>(validaciones);

            return result;
        }

        private async Task<List<ValidacionesRequisicionViewModel>> AlmacenarCambiosAsync(
            int idRequisicion,
            string userName,
            List<ValidaRequisicion> toAddOrUpdate)
        {
            var itemsSaved = await this.AddOrUpdateAsync(idRequisicion, userName, toAddOrUpdate).ConfigureAwait(false);
            var actual = itemsSaved.FirstOrDefault(v => v.StateValidation == EEstadoValidacion.Actual);
            this.SetPermisos(actual, string.Empty);

            return itemsSaved;
        }

        private async Task<List<ValidaRequisicion>> AutorizarRequisicionAsync(
            List<RequisicionPlazaViewModel> matriz,
            RequisicionPlazaViewModel requisicion)
        {
            var urlApi = this.configuracionGlobal.Configuration<string>("AutorizacionAPI");

            var response = await HttpRequestFactory.PutAsync($"{urlApi}/api/Autorizacion", new { matriz, requisicion })
                               .ConfigureAwait(false);

            var strResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var validaciones = JsonConvert.DeserializeObject<List<ValidaRequisicion>>(strResult);

            return validaciones;
        }

        private async Task<List<ValidacionesRequisicionViewModel>> ConvertValidacionesToMatrizAsync(
            IEnumerable<ValidaRequisicion> validaciones)
        {
            var res = new List<ValidacionesRequisicionViewModel>();

            foreach (var autorizacion in validaciones)
            {
                var user = await this.userService.GetUserByUserNameAsync(autorizacion.AprobadorUserName)
                               .ConfigureAwait(false);

                res.Add(
                    new ValidacionesRequisicionViewModel
                    {
                        Info = $"{user.Nombre} {user.Apellidos}",
                        Name = autorizacion.NivelValidacion.ToString(),
                        Date = autorizacion.Fecha,
                        Description = autorizacion.Comentario,
                        UserName = autorizacion.AprobadorUserName,
                        Id = 0,
                        CanApprove = false,
                        CanDeny = false,
                        CanCancel = false,
                        StateValidation = autorizacion.EstadoValidacion,
                        Timelapse = string.Empty
                    });
            }

            return res;
        }

        private async Task<List<ValidaRequisicion>> GetMatrizAprobacionAsync(
            string userNameRequeridor,
            int idRequisicion,
            string motivoIngresoDescripcion)
        {
            var urlApi = this.configuracionGlobal.Configuration<string>("AutorizacionAPI");
            var response = await HttpRequestFactory.GetAsync(
                                   $"{urlApi}/api/Autorizacion/{userNameRequeridor}/{motivoIngresoDescripcion}")
                               .ConfigureAwait(false);

            var strResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var validaciones = JsonConvert.DeserializeObject<List<ValidaRequisicion>>(strResult);

            var asyncRepository = this.validacionRepository;
            if (asyncRepository == null)
            {
                return validaciones;
            }

            var matriz = await asyncRepository.ListAsync(new ValidaRequisicionSpecification(idRequisicion))
                             .ConfigureAwait(false);

            foreach (var validaRequisicion in matriz)
            {
                var item = validaciones?.FirstOrDefault(
                    v => v != null && v.NivelValidacion == validaRequisicion.NivelValidacion);

                if (item == null)
                {
                    validaciones.Add(
                        new ValidaRequisicion
                        {
                            Id = validaRequisicion.Id,
                            Fecha = validaRequisicion.Fecha,
                            CreatedBy = validaRequisicion.CreatedBy,
                            Created = validaRequisicion.Created,
                            EditedBy = validaRequisicion.EditedBy,
                            Edited = validaRequisicion.Edited,
                            EstadoValidacion = validaRequisicion.EstadoValidacion,
                            Comentario = validaRequisicion.Comentario,
                            NivelValidacion = validaRequisicion.NivelValidacion,
                            AprobadorUserName = validaRequisicion.AprobadorUserName
                        });
                }
                else
                {
                    item.Id = validaRequisicion.Id;

                    if (!validaRequisicion.Fecha.HasValue)
                    {
                        continue;
                    }

                    item.Fecha = validaRequisicion.Fecha;
                    item.CreatedBy = validaRequisicion.CreatedBy;
                    item.Created = validaRequisicion.Created;
                    item.EditedBy = validaRequisicion.EditedBy;
                    item.Edited = validaRequisicion.Edited;
                    item.EstadoValidacion = validaRequisicion.EstadoValidacion;
                    item.Comentario = validaRequisicion.Comentario;
                }
            }

            return validaciones;
        }

        private async Task<bool> NotificarAsync(
            int idRequisicion,
            ValidacionesRequisicionViewModel validated,
            ValidacionesRequisicionViewModel toValidate,
            ETipoEvento tipoNotificacion)
        {
            User user = null;
            string toEmail = null;

            if (toValidate != null)
            {
                user = await this.userService.GetUserByUserNameAsync(toValidate.UserName).ConfigureAwait(false);
            }
            else
            {
                var coordinadorRS = this.configuracionGlobal.Configuration<string>("UserCoordinadorRS");
                user = await this.userService.GetUserByUserNameAsync(coordinadorRS).ConfigureAwait(false);

                if (tipoNotificacion == ETipoEvento.SolicitarAutorizacion)
                {
                    return true;
                }
            }

            toEmail = user.Mail;

            var notificacion = new NotificacionViewModel
            {
                ToMail = new List<string> { toEmail },
                TipoEvento = tipoNotificacion,
                Item = new
                {
                    user,
                    idRequisicion,
                    tipoNotificacion,
                    fecha = DateTime.Now,
                    comentario = validated?.Description
                }
            };

            return await this.notificarService.NotificarAsync(notificacion).ConfigureAwait(false);
        }

        private void SetPermisos(
            ValidacionesRequisicionViewModel validacionActual,
            string currentUserName,
            bool isAdmin = false,
            bool isCanceled = false)
        {
            if (validacionActual == null)
            {
                return;
            }

            if (isCanceled)
            {
                return;
            }

            if (!isAdmin)
            {
                if (currentUserName != string.Empty)
                {
                    if (validacionActual.UserName?.ToUpper() != currentUserName?.ToUpper())
                    {
                        validacionActual.CanApprove = false;
                        validacionActual.CanDeny = false;
                        validacionActual.CanCancel = false;
                        return;
                    }
                }
            }

            switch (validacionActual.NivelValidacion)
            {
                case ENivelValidacion.Requeridor:
                    validacionActual.CanApprove = false;
                    validacionActual.CanCancel = false;
                    validacionActual.CanDeny = false;
                    break;
                case ENivelValidacion.Presupuesto:
                case ENivelValidacion.DireccionGeneral:
                    validacionActual.CanApprove = true;
                    validacionActual.CanDeny = true;
                    validacionActual.CanCancel = true;
                    break;
                default:
                    validacionActual.CanApprove = true;
                    validacionActual.CanDeny = true;
                    validacionActual.CanCancel = false;
                    break;
            }

            validacionActual.StateValidation = EEstadoValidacion.Actual;
        }

        Task<List<ValidaRequisicion>> IAutorizacionService.SetPermisosAsync(List<ValidacionesRequisicionViewModel> matrizAprobacion, string currentUserUserName, bool isAdmin)
        {
            throw new NotImplementedException();
        }

        private List<ValidaRequisicion> TopeSalarial(List<ValidaRequisicion> validaciones)
        {
            var result = new List<ValidaRequisicion>();

            result = validaciones.Any(
                         v => v.NivelValidacion == ENivelValidacion.TopeSalarialDireccionRH
                              && v.EstadoValidacion == EEstadoValidacion.Rechazada)
                         ? validaciones.Where(v => v != null && v.EstadoValidacion != EEstadoValidacion.Actual).ToList()
                         : validaciones;

            return result;
        }

        private async Task<List<ValidacionesRequisicionViewModel>> UpdateAsync(
            int idRequisicion,
            string userName,
            List<ValidaRequisicion> validaciones)
        {
            foreach (var validacion in validaciones)
            {
                var toEdit = await this.validacionRepository.GetByIdAsync(validacion.Id).ConfigureAwait(false);

                toEdit.RequisicionId = idRequisicion;
                validacion.Active = toEdit.Active;
                validacion.RequisicionId = idRequisicion;
                validacion.Created = toEdit.Created;
                validacion.CreatedBy = toEdit.CreatedBy;
                validacion.Edited = DateTime.Now;
                validacion.EditedBy = validacion.AprobadorUserName;

                if (toEdit.EstadoValidacion == EEstadoValidacion.Aprobada)
                {
                    continue;
                }

                this.validacionRepository.ApplyCurrentValues(toEdit, validacion);
                await this.validacionRepository.UpdateAsync(toEdit).ConfigureAwait(false);
            }

            var result = this.mapper.Map<List<ValidacionesRequisicionViewModel>>(validaciones);

            return result;
        }

        private async Task<List<ValidaRequisicion>> ValidarRequisicionAsync(RequisicionPlazaViewModel requisicion)
        {
            var urlApi = this.configuracionGlobal.Configuration<string>("AutorizacionAPI");

            var response = await HttpRequestFactory.PutAsync($"{urlApi}/api/Autorizacion", requisicion)
                               .ConfigureAwait(false);

            var strResult = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var validaciones = JsonConvert.DeserializeObject<List<ValidaRequisicion>>(strResult);

            return validaciones;
        }
    }
}
