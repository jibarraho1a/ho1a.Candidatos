using ho1a.applicationCore.Data.Interfaces;
using ho1a.reclutamiento.models.Candidatos;
using ho1a.reclutamiento.models.Catalogos;
using ho1a.reclutamiento.models.ODS;
using ho1a.reclutamiento.services.Data.Interfaces;
using ho1a.reclutamiento.services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IODSQueries odsQueries;
        private readonly IAsyncRepository<Empresa> empresaRepository;
        private readonly IAsyncRepository<EstadoCivil> estadoCivilRepository;
        private readonly IAsyncRepository<Localidad> localidadRepository;
        private readonly IAsyncRepository<MotivoIngreso> motivoIngresoRepository;
        private readonly IAsyncRepository<PuestoSolicitado> puestoSolicitadoRepository;
        private readonly IGeneralService<PuestoSolicitado> puestoSolicitadoService;
        private readonly IAsyncRepository<ReferenciaVacante> referenciaVacanteRepository;
        private readonly IAsyncRepository<Salario> salarioRepository;
        private readonly IAsyncRepository<TipoPlaza> tipoPlazaRepository;

        public CatalogService(
            IAsyncRepository<Empresa> empresaRepository,
            IAsyncRepository<TipoPlaza> tipoPlazaRepository,
            IAsyncRepository<Localidad> localidadRepository,
            IAsyncRepository<MotivoIngreso> motivoIngresoRepository,
            IAsyncRepository<PuestoSolicitado> puestoSolicitadoRepository,
            IGeneralService<PuestoSolicitado> puestoSolicitadoService,
            IAsyncRepository<EstadoCivil> estadoCivilRepository,
            IAsyncRepository<Salario> salarioRepository,
            IAsyncRepository<ReferenciaVacante> referenciaVacanteRepository,
            IODSQueries odsQueries)
        {
            this.empresaRepository = empresaRepository;
            this.tipoPlazaRepository = tipoPlazaRepository;
            this.localidadRepository = localidadRepository;
            this.motivoIngresoRepository = motivoIngresoRepository;
            this.puestoSolicitadoRepository = puestoSolicitadoRepository;
            this.puestoSolicitadoService = puestoSolicitadoService;
            this.estadoCivilRepository = estadoCivilRepository;
            this.salarioRepository = salarioRepository;
            this.referenciaVacanteRepository = referenciaVacanteRepository;
            this.odsQueries = odsQueries;
        }

        public async Task<List<PuestoSolicitado>> GetListPuestosAsync()
        {
            var response = await this.UpdatePuestosAdamAsync();

            return response;
        }

        public async Task<IEnumerable<SelectListItem>> GetSelectListEmpresaAsync()
        {
            var response = await this.empresaRepository.ListAllAsync()
                               .ConfigureAwait(false);

            var items = new List<SelectListItem>
                            {
                                new SelectListItem
                                    {
                                        Value = null,
                                        Text = "SELECCIONE EMPRESA",
                                        Selected = true
                                    }
                            };

            foreach (var item in response)
            {
                items.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Descripcion });
            }

            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetSelectListEstadosCivilAsync()
        {
            var response = await this.estadoCivilRepository.ListAllAsync()
                               .ConfigureAwait(false);

            var items = new List<SelectListItem>
                            {
                                new SelectListItem
                                    {
                                        Value = null,
                                        Text = "SELECCIONE ESTADO CIVIL",
                                        Selected = true
                                    }
                            };

            foreach (var item in response)
            {
                items.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Nombre });
            }

            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetSelectListLocalidadesAsync()
        {
            var response = await this.localidadRepository.ListAllAsync()
                               .ConfigureAwait(false);

            var items = new List<SelectListItem>
                            {
                                new SelectListItem
                                    {
                                        Value = null,
                                        Text = "SELECCIONE LOCALIDAD",
                                        Selected = true
                                    }
                            };

            foreach (var item in response)
            {
                items.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Descripcion });
            }

            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetSelectListMotivosIngresoAsync()
        {
            var response = await this.motivoIngresoRepository.ListAllAsync()
                               .ConfigureAwait(false);

            var items = new List<SelectListItem>
                            {
                                new SelectListItem
                                    {
                                        Value = null,
                                        Text = "SELECCIONE MOTIVO INGRESO",
                                        Selected = true
                                    }
                            };

            foreach (var item in response)
            {
                items.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Descripcion });
            }

            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetSelectListPuestos()
        {
            var response = this.GetPuestosAdamAsync()
                .Result;

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
                response.Select(item => new SelectListItem { Value = item?.Id.ToString(), Text = item?.Nombre }));

            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetSelectListReferenciaVancanteAsync()
        {
            var response = await this.referenciaVacanteRepository.ListAllAsync()
                               .ConfigureAwait(false);

            var items = new List<SelectListItem>
            {
                new SelectListItem
                    {
                        Value = null,
                        Text = "¿DONDÉ SE ENTERO DE LA VACANTE?",
                        Selected = true
                    }
            };

            foreach (var item in response)
            {
                items.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Nombre });
            }

            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetSelectListTiposPlazaAsync()
        {
            var response = await this.tipoPlazaRepository.ListAllAsync()
                               .ConfigureAwait(false);

            var items = new List<SelectListItem>
                            {
                                new SelectListItem
                                    {
                                        Value = null,
                                        Text = "SELECCIONE TIPO DE PLAZA",
                                        Selected = true
                                    }
                            };

            items.AddRange(
                response.Select(item => new SelectListItem { Value = item.Id.ToString(), Text = item.Descripcion }));

            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetSelectListUltimoSalarioAsync()
        {
            var response = await this.salarioRepository.ListAllAsync()
                               .ConfigureAwait(false);

            var items = new List<SelectListItem>
                            {
                                new SelectListItem
                                    {
                                        Value = null,
                                        Text = "SELECCIONE RANGO DE SALARIO",
                                        Selected = true
                                    }
                            };

            foreach (var item in response)
            {
                items.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.ToString() });
            }

            return items;
        }

        private Task<List<PuestoSolicitado>> GetPuestosAdamAsync() => this.puestoSolicitadoRepository.ListAllAsync();

        Task<IEnumerable<SelectListItem>> ICatalogService.GetSelectListPuestos()
        {
            throw new NotImplementedException();
        }

        private async Task UpdatePuestoAsync(IList<Puestos> puestosAdam)
        {
            var result = await this.puestoSolicitadoService.GetAsync()
                             .ConfigureAwait(true);

            var puestosSolicitados = result.ToList();

            foreach (var puesto in puestosAdam)
            {
                var puestoSolicitado = puestosSolicitados.FirstOrDefault(p => p.AdamId == puesto.Puesto);

                if (puestoSolicitado == null)
                {
                    puestoSolicitado = new PuestoSolicitado { AdamId = puesto.Puesto, Nombre = puesto.Descripcion };

                    await this.puestoSolicitadoRepository.AddAsync(puestoSolicitado)
                        .ConfigureAwait(false);
                }
                else
                {
                    puestoSolicitado.Nombre = puesto.Descripcion;

                    var toEdit = await this.puestoSolicitadoRepository.GetByIdAsync(puestoSolicitado.Id)
                                     .ConfigureAwait(false);
                    this.puestoSolicitadoRepository.ApplyCurrentValues(toEdit, puestoSolicitado);
                    await this.puestoSolicitadoRepository.UpdateAsync(puestoSolicitado)
                        .ConfigureAwait(false);
                }
            }
        }

        private async Task<List<PuestoSolicitado>> UpdatePuestosAdamAsync()
        {
            var puestosAdam = await this.odsQueries.GetPuestosAsync()
                                  .ConfigureAwait(false);

            var responseMiRh = await this.puestoSolicitadoRepository.ListAllAsync()
                                   .ConfigureAwait(false);

            foreach (var puestoSolicitado in responseMiRh)
            {
                puestoSolicitado.Active = false;
            }

            foreach (var puestose in puestosAdam)
            {
                var toEdit = responseMiRh.FirstOrDefault(p => p.AdamId.Trim() == puestose.Puesto.Trim());

                if (toEdit != null)
                {
                    toEdit.Active = true;
                    toEdit.Nombre = puestose.Descripcion;
                    await this.puestoSolicitadoRepository.UpdateAsync(toEdit)
                        .ConfigureAwait(false);
                }
                else
                {
                    var toInsert = new PuestoSolicitado
                    {
                        Nombre = puestose.Descripcion,
                        AdamId = puestose.Puesto.Trim()
                    };

                    await this.puestoSolicitadoRepository.AddAsync(toInsert)
                        .ConfigureAwait(false);
                }
            }

            return await this.puestoSolicitadoRepository.ListAllAsync()
                       .ConfigureAwait(false);
        }
    }
}