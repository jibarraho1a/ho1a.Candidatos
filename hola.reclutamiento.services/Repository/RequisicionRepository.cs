using ho1a.applicationCore.Data;
using ho1a.applicationCore.Entities;
using ho1a.reclutamiento.services.Data;

namespace ho1a.reclutamiento.services.Repository
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
