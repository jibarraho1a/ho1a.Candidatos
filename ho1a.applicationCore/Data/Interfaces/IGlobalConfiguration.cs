using System.Collections.Generic;

namespace ho1a.applicationCore.Data.Interfaces
{
    public interface IGlobalConfiguration<TEntity>
    {
        List<TEntity> Configuraciones { get; set; }
        T Configuration<T>(string key);
    }
}
