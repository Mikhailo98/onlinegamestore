using DAL.Repository;
using Domain;
using Domain.Repository;

namespace DAL
{
    internal class PlatformTypeRepository : Repository<PlatformType, int>, IPlatformTypeRepository
    {
        public PlatformTypeRepository(ApplicationContext context) : base(context)
        {
        }
    }
}