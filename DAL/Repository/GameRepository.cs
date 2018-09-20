using System.Collections.Generic;
using Domain;
using Domain.Repository;
using DAL.Repository;

namespace DAL
{
    class GameRepository : Repository<Game, int>, IGameRepository
    {
        private ApplicationContext context;

        public GameRepository(ApplicationContext context) : base(context)
        {
            this.context = context;
        }


    }
}