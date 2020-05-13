using ho1a.applicationCore.Data.Interfaces;
using ho1a.reclutamiento.models.Plazas;
using ho1a.reclutamiento.services.Data.Interfaces;
using ho1a.reclutamiento.services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ho1a.reclutamiento.services.Services
{
    public class PlantillaEntrevistaService : GeneralService<PlantillaEntrevista>, IPlantillaEntrevistaService
    {
        public PlantillaEntrevistaService(
            IAsyncRepository<PlantillaEntrevista> asyncRepository,
            IRepository<PlantillaEntrevista> repository)
            : base(asyncRepository, repository)
        {
        }
    }
}