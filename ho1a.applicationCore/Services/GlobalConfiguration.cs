using ho1a.applicationCore.Data.Interfaces;
using ho1a.applicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ho1a.applicationCore.Services
{
    public class GlobalConfiguration<TEntity> : IGlobalConfiguration<TEntity>
        where TEntity : ConfiguracionBase
    {
        private readonly IAsyncRepository<TEntity> configuracionRepository;

        public GlobalConfiguration(IAsyncRepository<TEntity> configuracionRepository)
        {
            this.configuracionRepository = configuracionRepository;
            this.Initialize();
        }

        /// <inheritdoc />
        public List<TEntity> Configuraciones { get; set; }

        public T Configuration<T>(string key)
        {
            var result =
                this.Configuraciones?.FirstOrDefault(x => string.Equals(((string)x.Key).ToUpper(), key.ToUpper()))
                    ?.Values;

            if (result != null)
            {
                var v = (T)Convert.ChangeType(result, typeof(T));
                return result == null ? default(T) : v;
            }

            return default(T);
        }

        private void Initialize()
        {
            var configuraciones = this.configuracionRepository.ListAllAsync()
                .Result;
            this.Configuraciones = configuraciones.ToList<TEntity>();
        }
    }
}
