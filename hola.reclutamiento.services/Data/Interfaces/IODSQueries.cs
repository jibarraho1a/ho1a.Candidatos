using ho1a.reclutamiento.models.ODS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Data.Interfaces
{
    public interface IODSQueries
    {
        Task<IEnumerable<DatosAgrTrab>> GetAllAliasAsync();

        Task<IEnumerable<DatosAgrTrab>> GetAliasByPuestoAsync(int idTrabajador, string idPuesto);

        Task<IEnumerable<Plazas>> GetAllPlazasByPuestoAsync(int idTrabajador);

        Task<IEnumerable<Plazas>> GetAllPlazasByPuestoAsync(int idTrabajador, string idAlias);

        Task<RelTrabAgr> GetColaboradorByAgrupacionAndUserNameAsync(string agrupacion, string idColaborador);

        Task<IEnumerable<RelTrabAgr>> GetColaboradorByIdAsync(string idTrabajador);

        Task<IEnumerable<RelTrabAgr>> GetColaboradoresByAreaAsync(string area);

        Task<IEnumerable<RelTrabAgr>> GetColaboradoresByDepartamentoAsync(string departamento);

        Task<List<int>> GetDirectorByDireccionAsync(string direccion);

        Task<IEnumerable<Plazas>> GetPlazasByPuestoAsync(int idTrabajador);

        Task<IEnumerable<Plazas>> GetPlazasByPuestoAsync(int idTrabajador, string idAlias);

        Task<IEnumerable<Puestos>> GetPuestosAsync();

        Task<string> GetTabuladorByIdColaboradorIdPuesto(int idColaborador, string idPuesto);

        Task<int?> GetTrabajadorByPuestoAsync(string idPuesto);
    }
}
