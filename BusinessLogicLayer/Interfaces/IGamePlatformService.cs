using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace BusinessLogicLayer.Services
{
    public interface IGamePlatformService
    {
        void AddPlatforms(List<int> platforms, ref Game game);
        Task DeletePlatforms(List<int> platformsId, Game entity);
    }
}