using DAL;
using DAL.Repository;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Repository
{
    class GameGenreRepository : Repository<GenreGame, int>, IGameGenreRepository
    {

        public GameGenreRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
