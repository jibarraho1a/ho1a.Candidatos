using ho1a.applicationCore.Data.Interfaces;
using ho1a.reclutamiento.models.Configuracion;
using ho1a.reclutamiento.services.Data.Interfaces;
using ho1a.reclutamiento.services.Services.Interfaces;
using ho1a.reclutamiento.services.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services
{
    public class ConfiguracionGlobalService : IConfiguracionGlobalService
    {
        private readonly IAsyncRepository<Configuracion> configuracionRepository;

        public ConfiguracionGlobalService(IAsyncRepository<Configuracion> configuracionRepository)
        {
            this.configuracionRepository = configuracionRepository;
        }

        public async Task<Configuracion> AddConfiguracionesGlobalesAsync(Configuracion configuracion)
        {
            Configuracion result;
            try
            {
                result = await this.configuracionRepository.AddAsync(configuracion)
                                   .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return result;
        }

        public async Task<bool> DeleteConfiguracionGlobalAsync(int idConfiguracionGlobal)
        {
            try
            {
                var toEdit = await this.configuracionRepository.GetByIdAsync(idConfiguracionGlobal)
                                       .ConfigureAwait(false);
                toEdit.Active = false;

                await this.configuracionRepository.UpdateAsync(toEdit)
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return true;
        }

        public async Task<List<Configuracion>> GetConfiguracionGlobalAsync()
        {
            List<Configuracion> result;
            try
            {
                result = await this.configuracionRepository.ListAsync(new ConfiguracionGlobalSpecification())
                                   .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return result;
        }

        public async Task<Configuracion> GetConfiguracionGlobalAsync(int idConfiguracionGlobal)
        {
            Configuracion result = null;
            try
            {
                result = await this.configuracionRepository.GetByIdAsync(idConfiguracionGlobal)
                                   .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return result;
        }

        public async Task<Configuracion> UpdateConfiguracionGlobalAsync(
            int idConfiguracionGlobal,
            Configuracion configuracionGlobal)
        {
            Configuracion toEdit;
            try
            {
                toEdit = await this.configuracionRepository.GetByIdAsync(idConfiguracionGlobal)
                                   .ConfigureAwait(false);

                toEdit.Descripcion = configuracionGlobal.Descripcion;
                toEdit.Key = configuracionGlobal.Key;
                toEdit.Values = configuracionGlobal.Values;

                await this.configuracionRepository.UpdateAsync(toEdit)
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return toEdit;
        }
    }
}
