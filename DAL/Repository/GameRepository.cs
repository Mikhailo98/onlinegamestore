using System.Collections.Generic;
using Domain;
using Domain.Repository;
using DAL.Repository;

namespace DAL
{
    class GameRepository : Repository<Game, int>, IGameRepository
    {

        public GameRepository(ApplicationContext context) : base(context)
        {
        }


    }
}