using ho1a.applicationCore.Data.Interfaces;
using ho1a.reclutamiento.models.Catalogos;
using ho1a.reclutamiento.models.ODS;
using ho1a.reclutamiento.models.Plazas;
using ho1a.reclutamiento.models.Seguridad;
using ho1a.reclutamiento.services.Data.Interfaces;
using ho1a.reclutamiento.services.Services.Interfaces;
using ho1a.reclutamiento.services.Specifications;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services
{
    public class InformacionTrabajoService : IInformacionTrabajoService
    {
        private readonly IODSQueries odsQueries;
        private readonly IAsyncRepository<PuestoSolicitado> puestoSolicitadoRepository;
        private readonly IAsyncRepository<PuestoTabulador> puestoTabuladoRepository;
        private readonly IAsyncRepository<TabuladorSalario> tabuladorRepository;
        private readonly IUserService userService;

        public InformacionTrabajoService(
            IODSQueries odsQueries,
            IUserService userService,
            IAsyncRepository<PuestoSolicitado> puestoSolicitadoRepository,
            IAsyncRepository<PuestoTabulador> puestoTabuladoRepository,
            IAsyncRepository<TabuladorSalario> tabuladorRepository)
        {
            this.odsQueries = odsQueries;
            this.userService = userService;
            this.puestoSolicitadoRepository = puestoSolicitadoRepository;
            this.puestoTabuladoRepository = puestoTabuladoRepository;
            this.tabuladorRepository = tabuladorRepository;
        }

        public async Task<List<SelectListItem>> GetAliasByTrabajadorPuestoAsync(int idTrabajador, string idPuesto)
        {
            var response = await this.odsQueries.GetAliasByPuestoAsync(idTrabajador, idPuesto)
                                     .ConfigureAwait(false);

            var items = new List<SelectListItem>
                            {
                                new SelectListItem
                                    {
                                        Value = null,
                                        Text = "SELECCIONE ALIAS",
                                        Selected = true
                                    }
                            };
            items.AddRange(response.Select(item => new SelectListItem { Value = item.Dato, Text = item.Descripcion }));

            return items;
        }

        public async Task<List<SelectListItem>> GetAllCatalogoPuestos(int idTrabajador)
        {
            var plazas = await this.GetAllVacantesByIdTrabajadorAsync(idTrabajador)
                                   .ConfigureAwait(false);

            var response = await this.puestoSolicitadoRepository.ListAllAsync()
                                     .ConfigureAwait(false);

            var puestos = new List<PuestoSolicitado>();

            foreach (var plaza in plazas)
            {
                var puesto = response.FirstOrDefault(p => p?.AdamId == plaza.Puesto.ToString());
                if (puesto != null)
                {
                    puestos.Add(puesto);
                }
            }

            var items = new List<SelectListItem>
                            {
                                new SelectListItem
                                    {
                                        Value = null,
                                        Text = "SELECCIONE PUESTO",
                                        Selected = true
                                    }
                            };

            items.AddRange(
                puestos.Select(item => new SelectListItem { Value = item?.Id.ToString(), Text = item?.Nombre }));

            return items;
        }

        public async Task<List<SelectListItem>> GetAllCatalogoPuestos(int idColaborador, string idAlias)
        {
            var plazas = await this.GetAllVacantesByIdTrabajadorAsync(idColaborador)
                                   .ConfigureAwait(false);

            var response = await this.puestoSolicitadoRepository.ListAllAsync()
                                     .ConfigureAwait(false);

            var puestos = new List<PuestoSolicitado>();

            foreach (var plaza in plazas)
            {
                var puesto = response.FirstOrDefault(p => p?.AdamId == plaza.Puesto.ToString());
                puestos.Add(puesto);
            }

            var items = new List<SelectListItem>
                            {
                                new SelectListItem
                                    {
                                        Value = null,
                                        Text = "SELECCIONE PUESTO",
                                        Selected = true
                                    }
                            };

            items.AddRange(
                puestos.Select(item => new SelectListItem { Value = item?.Id.ToString(), Text = item?.Nombre }));

            return items;
        }

        public async Task<List<SelectListItem>> GetCatalogoPuestos(int idTrabajador)
        {
            var plazas = await this.GetVacantesByIdTrabajadorAsync(idTrabajador)
            .ConfigureAwait(false);

            var response = await this.puestoSolicitadoRepository.ListAllAsync()
                                     .ConfigureAwait(false);

            var puestos = new List<PuestoSolicitado>();

            foreach (var plaza in plazas)
            {
                var puesto = response.FirstOrDefault(p => p?.AdamId == plaza.Puesto.ToString());

                if (puesto != null)
                {
                    puestos.Add(puesto);
                }
            }

            var items = new List<SelectListItem>
                            {
                                new SelectListItem
                                    {
                                        Value = null,
                                        Text = "SELECCIONE PUESTO",
                                        Selected = true
                                    }
                            };

            items.AddRange(
                puestos.Select(item => new SelectListItem { Value = item?.Id.ToString(), Text = item?.Nombre }));

            return items;
        }

        public async Task<List<SelectListItem>> GetCatalogoPuestos(int idTrabajador, string idAlias)
        {
            var plazas = await this.GetVacantesByIdTrabajadorAsync(idTrabajador, idAlias)
                                   .ConfigureAwait(false);

            var response = await this.puestoSolicitadoRepository.ListAllAsync()
                                     .ConfigureAwait(false);

            var puestos = new List<PuestoSolicitado>();

            foreach (var plaza in plazas)
            {
                var puesto = response.FirstOrDefault(p => p?.AdamId == plaza.Puesto.ToString());
                puestos.Add(puesto);
            }

            var items = new List<SelectListItem>
                            {
                                new SelectListItem
                                    {
                                        Value = null,
                                        Text = "SELECCIONE PUESTO",
                                        Selected = true
                                    }
                            };

            items.AddRange(
                puestos.Select(item => new SelectListItem { Value = item?.Id.ToString(), Text = item?.Nombre }));

            return items;
        }

        public async Task<InformacionTrabajo> GetInformacionTrabajoAsync(int idColaborador, int idPuesto, string idAlias)
        {
            var plazas = await this.GetAllVacantesByIdTrabajadorAsync(idColaborador)
                                   .ConfigureAwait(false);

            var puestoMiRh = await this.puestoSolicitadoRepository.GetByIdAsync(idPuesto)
                                       .ConfigureAwait(false);

            var plaza = plazas.FirstOrDefault(p => p != null && p.Puesto.ToString() == puestoMiRh.AdamId);

            var userReferencia = await this.UsuarioReferencia(puestoMiRh.AdamId).ConfigureAwait(false);

            var tabulador = await this.GetTabuladorAsync(idColaborador, idPuesto)
                                       .ConfigureAwait(false);

            var infoTrabajo = new InformacionTrabajo
            {
                PuestoSolicitado = puestoMiRh.Nombre,
                Mercado = string.Empty,
                Departamento = userReferencia?.Departamento ?? string.Empty,
                Descripcion = puestoMiRh.Descripcion,
                Responsabilidades = puestoMiRh.Responsabilidad,
                NumeroVacantes = plaza.NumeroVacantes,
                Tabulador = tabulador,
                NivelOrganizacional = userReferencia?.NivelOrganizacional
            };

            return infoTrabajo;
        }

        public async Task<TabuladorSalario> GetTabuladorAsync(int idColaborador, int idPuesto)
        {
            var puesto = await this.puestoSolicitadoRepository.GetByIdAsync(idPuesto)
                                   .ConfigureAwait(false);
            var tabulador = await this.odsQueries.GetTabuladorByIdColaboradorIdPuesto(idColaborador, puesto.AdamId)
                                      .ConfigureAwait(false);
            TabuladorSalario tabuladorSalario = null;
            if (tabulador == null || tabulador.Trim()
                                         .Equals("N/A"))
            {
                var puestoTabulador = await this.puestoTabuladoRepository.Single(
                    new PuestoTabuladorSpecification(idPuesto))
                                                .ConfigureAwait(false);

                tabulador = puestoTabulador?.Tabulador?.Tabulador;
            }

            tabuladorSalario = await this.tabuladorRepository.Single(new TabuladorSalarioSpecification(tabulador))
                                             .ConfigureAwait(false);

            return tabuladorSalario;
        }

        public async Task<List<Plazas>> GetVacantesByIdTrabajadorAsync(int idColaborador)
        {
            var vacantes = await this.odsQueries.GetPlazasByPuestoAsync(idColaborador)
                                     .ConfigureAwait(false);

            return vacantes.ToList();
        }

        public async Task<List<Plazas>> GetVacantesByIdTrabajadorAsync(int idColaborador, string idAlias)
        {
            var vacantes = await this.odsQueries.GetPlazasByPuestoAsync(idColaborador, idAlias)
                                     .ConfigureAwait(false);

            return vacantes.ToList();
        }

        public async Task<User> UsuarioReferencia(string idPuesto)
        {
            var idColaboradorReferencia = await this.odsQueries.GetTrabajadorByPuestoAsync(idPuesto)
                                                    .ConfigureAwait(false);

            if (idColaboradorReferencia == null)
            {
                return null;
            }

            var userReferencia = await this.userService.GetUserByIdColaboradorAsync(idColaboradorReferencia.Value)
                                           .ConfigureAwait(false);

            return userReferencia;
        }

        private async Task<List<Plazas>> GetAllVacantesByIdTrabajadorAsync(int idTrabajador)
        {
            var vacantes = await this.odsQueries.GetAllPlazasByPuestoAsync(idTrabajador)
                                     .ConfigureAwait(false);

            return vacantes.ToList();
        }

        private async Task<List<Plazas>> GetAllVacantesByIdTrabajadorAsync(int idTrabajador, string idAlias)
        {
            var vacantes = await this.odsQueries.GetAllPlazasByPuestoAsync(idTrabajador, idAlias)
                                     .ConfigureAwait(false);

            return vacantes.ToList();
        }
    }
}