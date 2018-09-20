using DAL.Repository;
using Domain;
using Domain.Repository;

namespace DAL
{
    internal class PlatformTypeRepository : Repository<PlatformType, int>, IPlatformTypeRepository
    {
        private ApplicationContext context;

        public PlatformTypeRepository(ApplicationContext context) : base(context)
        {
            this.context = context;
        }
    }
}