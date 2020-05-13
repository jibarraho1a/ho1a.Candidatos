using ho1a.applicationCore.Data.Interfaces;
using ho1a.reclutamiento.enums.Notificacion;
using ho1a.reclutamiento.enums.Plazas;
using ho1a.reclutamiento.models.Configuracion;
using ho1a.reclutamiento.models.Plazas;
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
    public class EntrevistaService : GeneralService<Entrevista>, IEntrevistaService
    {
        private readonly IAsyncRepository<Competencia> competenciaRepository;
        private readonly IGlobalConfiguration<Configuracion> configuration;
        private readonly IAsyncRepository<Entrevista> entrevistaRepository;
        private readonly INotificarService notificarService;
        private readonly IPlantillaEntrevistaService plantillaEntrevistaService;
        private readonly IRequisicionService requisicionService;
        private readonly IAsyncRepository<TernaCandidato> ternaCandidatoRepository;
        private readonly IUserService userService;

        public EntrevistaService(
            IAsyncRepository<Entrevista> entrevistaAsyncRepository,
            IAsyncRepository<Competencia> competenciaRepository,
            IRepository<Entrevista> entrevistaRepository,
            IAsyncRepository<TernaCandidato> ternaCandidatoRepository,
            IPlantillaEntrevistaService plantillaEntrevistaService,
            INotificarService notificarService,
            IGlobalConfiguration<Configuracion> configuration,
            IUserService userService,
            IRequisicionService requisicionService)
            : base(entrevistaAsyncRepository, entrevistaRepository)
        {
            this.entrevistaRepository = entrevistaAsyncRepository;
            this.competenciaRepository = competenciaRepository;
            this.ternaCandidatoRepository = ternaCandidatoRepository;
            this.requisicionService = requisicionService;
            this.plantillaEntrevistaService = plantillaEntrevistaService;
            this.notificarService = notificarService;
            this.configuration = configuration;
            this.userService = userService;
        }

        public async Task<List<Ternas>> AddCompetencia(
            int idRequisicion,
            int idEntrevista,
            Competencia competenciaToEdit)
        {
            var entrevistaToEdit = await this.entrevistaRepository.GetByIdAsync(idEntrevista).ConfigureAwait(false);

            if (competenciaToEdit != null) entrevistaToEdit.Competencias.Add(competenciaToEdit);

            await this.entrevistaRepository.UpdateAsync(entrevistaToEdit).ConfigureAwait(false);

            return await this.GetEntrevistasByRequisicionAsync(idRequisicion).ConfigureAwait(false);
        }

        public async Task AddCompetenciasToEntrevistaFromPlantilla(int idRequisicion, int idEntrevista)
        {
            var plantillaEntrevistas = await this.plantillaEntrevistaService.GetAsync(
                                           new PlantillaEntrevistaSpecification(idRequisicion)).ConfigureAwait(false);

            var competencias = plantillaEntrevistas.Select(
                    x => new Competencia { Nombre = x.Nombre, Descripcion = x.Descripcion, PlantillaEntrevista = x })
                .ToList();

            var entrevista = await this.entrevistaRepository.Single(
                                 new EntrevistaSpecification(idRequisicion, idEntrevista)).ConfigureAwait(false);

            for (var i = 0; i < entrevista.Competencias.Count; i++)
            {
                var competencia = entrevista.Competencias.ToList()[i];
                await this.competenciaRepository.DeleteAsync(competencia).ConfigureAwait(false);
            }

            entrevista = await this.entrevistaRepository.Single(
                             new EntrevistaSpecification(idRequisicion, idEntrevista)).ConfigureAwait(false);

            entrevista.Competencias = competencias;

            await this.entrevistaRepository.UpdateAsync(entrevista).ConfigureAwait(false);
        }

        public async Task AddEntrevistadorInvitado(int idRequisicion, int idCandidato, Entrevista entrevista)
        {
            try
            {
                var ternas = await this.GetEntrevistasByRequisicionAsync(idRequisicion).ConfigureAwait(false);

                var terna = ternas.FirstOrDefault(t => t.TernaCandidato.Any(c => c?.CandidatoId == idCandidato));

                var ternaCandidato = terna.TernaCandidato.FirstOrDefault(t => t.CandidatoId == idCandidato);

                entrevista.TipoEntrevista = EEntrevista.Invitado;
                entrevista.Candidato = null;
                entrevista.CandidatoId = idCandidato;
                entrevista.Active = true;

                ternaCandidato.Entrevistas.Add(entrevista);

                await this.ternaCandidatoRepository.UpdateAsync(ternaCandidato).ConfigureAwait(false);

                await this.AddCompetenciasToEntrevistaFromPlantilla(idRequisicion, entrevista.Id).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        public async Task<List<Ternas>> GetEntrevistasByRequisicionAsync(int idRequisicion)
        {
            var result = await this.requisicionService.GetTernasByRequisicionAsync(idRequisicion).ConfigureAwait(false);
            return result;
        }

        public async Task<List<Ternas>> SetDateByRequisicionAndEntrevistaAsync(
            int idRequisicion,
            int idEntrevista,
            EntrevistaViewModel entrevista)
        {
            var entrevistaToEdit = await this.entrevistaRepository.Single(
                                       new EntrevistaSpecification(idRequisicion, idEntrevista)).ConfigureAwait(false);

            var notificarFechaInicioEntrevista = false;

            if (entrevistaToEdit.FechaInicioEntrevista == null)
            {
                notificarFechaInicioEntrevista = entrevista.FechaInicioEntrevista != null;
            }

            if (entrevista != null)
            {
                entrevistaToEdit.FechaInicioEntrevista = entrevista.FechaInicioEntrevista;

                entrevistaToEdit.FechaTerminoEntrevista = entrevista.Recomendable != null
                                                              ? (DateTime?)DateTime.Now
                                                              : null;

                entrevistaToEdit.Comentarios = entrevista.Comentarios;
                entrevistaToEdit.Fortalezas = entrevista.Fortalezas;
                entrevistaToEdit.Debilidades = entrevista.Debilidades;
                entrevistaToEdit.Recomendable = entrevista.Recomendable;

                if (entrevista.TipoEntrevistaId == EEntrevista.Invitado)
                {
                    entrevistaToEdit.Entrevistador = entrevista.EntrevistadorUserName;
                }

                foreach (var competencia in entrevistaToEdit.Competencias)
                {
                    var viewCompetencia = entrevista.Competencias.FirstOrDefault(c => c.Id == competencia.Id);
                    if (viewCompetencia != null)
                    {
                        competencia.Resultado = viewCompetencia.Resultado;
                    }
                }
            }

            await this.entrevistaRepository.UpdateAsync(entrevistaToEdit).ConfigureAwait(false);

            if (notificarFechaInicioEntrevista)
            {
                var requisicion = this.requisicionService.Single(new RequisicionSpecification(idRequisicion));
                var usernameRS = this.configuration.Configuration<string>("UserCoordinadorRS");
                var user = await this.userService.GetUserByUserNameAsync(entrevista.EntrevistadorUserName);

                var notificar = new NotificacionViewModel
                {
                    ToMail = new List<string> { user.Mail },
                    TipoEvento = ETipoEvento.NotificarEntrevistaEntrevistador,
                    Item = new { entrevistador = user, entrevista = entrevistaToEdit, requisicion }
                };

                await this.notificarService.NotificarAsync(notificar).ConfigureAwait(false);

                user = await this.userService.GetUserByUserNameAsync(requisicion.UserRequeridor);

                if (user.UserName.ToUpper() != entrevistaToEdit.Entrevistador.ToUpper())
                {
                    notificar = new NotificacionViewModel
                    {
                        ToMail = new List<string> { user.Mail },
                        TipoEvento = ETipoEvento.NotificacionTernaEntrevista,
                        Item = new { entrevistador = user, entrevista = entrevistaToEdit, requisicion }
                    };

                    await this.notificarService.NotificarAsync(notificar).ConfigureAwait(false);
                }

                if (usernameRS.ToUpper() != entrevistaToEdit.Entrevistador.ToUpper())
                {
                    var userRS = await this.userService.GetUserByUserNameAsync(usernameRS).ConfigureAwait(false);

                    var notificarRS = new NotificacionViewModel
                    {
                        ToMail = new List<string> { userRS.Mail },
                        TipoEvento = ETipoEvento.NotificacionRySEntrevista,
                        Item = new
                        {
                            entrevistador = userRS,
                            entrevista = entrevistaToEdit,
                            requisicion
                        }
                    };

                    await this.notificarService.NotificarAsync(notificarRS).ConfigureAwait(false);
                }

                if (entrevistaToEdit?.Candidato?.CandidatoUser?.Email != null)
                {
                    var notificarCandidato = new NotificacionViewModel
                    {
                        ToMail =
                                                         new List<string>
                                                             {
                                                                 entrevistaToEdit.Candidato.CandidatoUser.Email
                                                             },
                        TipoEvento = ETipoEvento.NotificacionCandidatoEntrevista,
                        Item = new
                        {
                            entrevistador = user,
                            entrevista = entrevistaToEdit,
                            requisicion
                        }
                    };

                    await this.notificarService.NotificarAsync(notificarCandidato).ConfigureAwait(false);
                }
            }

            return await this.GetEntrevistasByRequisicionAsync(idRequisicion).ConfigureAwait(false);
        }
    }
}