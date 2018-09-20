using BusinessLogicLayer.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.Dtos;
using AutoMapper;

namespace BusinessLogicLayer.Services
{
    internal class GameService : IGameService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;


        public GameService(IUnitOfWork unit, IMapper mapper)
        {
            unitOfWork = unit;
            this.mapper = mapper;
        }



        public async Task AddGame(GameDto gameDto)
        {
            using (unitOfWork)
            {
                if (unitOfWork.GameRepository.Get(g => g.Name == gameDto.Name) != null)
                {
                    throw new ArgumentException("Game with such name already exists");
                }

                if (unitOfWork.PublisherRepository.Get(p => p.Id == gameDto.PublisherId) == null)
                {
                    throw new ArgumentException("invalid publisher id");
                }

                var game = mapper.Map<Game>(gameDto);


                unitOfWork.GameRepository.Create(game);
                await unitOfWork.CommitAsync();



            }
        }

        public Task DeleteGame(int id)
        {
            throw new NotImplementedException();
        }

        public Task EditGame(int id, GameDto editedGame)
        {
            throw new NotImplementedException();
        }

        public async Task<GameDto> GetInfo(int id)
        {
            using (unitOfWork)
            {
                var gameEntity = unitOfWork.GameRepository
                    .GetSingle(filter: g => g.Id == id, includeProperties: "Publisher,Comments,GenreGames,GamePlatformTypes");
                if (gameEntity == null)
                {
                    throw new ArgumentException("Invalid game id");
                }

                return mapper.Map<GameDto>(gameEntity);
            }
        }
    }
}
