using AutoMapper;
using BusinessLogicLayer.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    class GamePlatformService : IGamePlatformService
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;


        public GamePlatformService(IUnitOfWork unit, IMapper mapper)
        {
            unitOfWork = unit;
            this.mapper = mapper;
        }


        public async Task DeletePlatforms(List<int> platformsId, Game entity)
        {
            foreach (var platform in entity.GamePlatformTypes)
            {
                unitOfWork.GamePlatformTypeRepository.Delete(platform);
            }

            // await AddPlatforms(platformsId, entity);
        }

        public void AddPlatforms(List<int> platforms, ref Game game)
        {
            foreach (var item in platforms)
            {
                var returnPlatform = unitOfWork
                    .PlatformTypeRepository.GetSingleAsync(i => i.Id == item).GetAwaiter().GetResult();

                if (returnPlatform == null)
                {
                    throw new ArgumentException($"Invalid platformtype id: {item}");
                }
                unitOfWork.GamePlatformTypeRepository.Create(new GamePlatformType() { Game = game, PlatformType = returnPlatform });
            }
        }


    }
}
