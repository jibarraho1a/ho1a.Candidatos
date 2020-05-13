using ho1a.applicationCore.Data;
using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.services.Data;

namespace Reclutamiento.Repository
{
    public class RequisicionRepository<T> : EfRepository<T>
        where T : BaseEntityId
    {
        public RequisicionRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
