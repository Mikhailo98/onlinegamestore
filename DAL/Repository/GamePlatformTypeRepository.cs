using DAL;
using DAL.Repository;
using Domain;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Repository
{
    class GamePlatformTypeRepository : Repository<GamePlatformType, int>, IGamePlatformTypeRepository
    {

        public GamePlatformTypeRepository(ApplicationContext context) : base(context)
        {

        }
    }
}