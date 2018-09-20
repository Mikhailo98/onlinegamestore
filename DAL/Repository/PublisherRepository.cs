using DAL.Repository;
using Domain;
using Domain.Repository;

namespace DAL
{
    internal class PublisherRepository : Repository<Publisher, int>, IPublisherRepository
    {
        private ApplicationContext context;

        public PublisherRepository(ApplicationContext context) : base(context)
        {
            this.context = context;
        }
    }
}