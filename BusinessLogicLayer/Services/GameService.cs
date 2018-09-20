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



        public async Task AddGame(CreateGameDto gameDto)
        {
            using (unitOfWork)
            {
                if (unitOfWork.GameRepository.GetSingle(g => g.Name == gameDto.Name && g.PublisherId == gameDto.PublisherId) != null)
                {
                    throw new ArgumentException("Game with such name of such publisher already exists");
                }

                if (unitOfWork.PublisherRepository.GetSingle(p => p.Id == gameDto.PublisherId) == null)
                {
                    throw new ArgumentException("invalid publisher id");
                }


                var game = mapper.Map<Game>(gameDto);

                CheckPlatforms(gameDto.Platforms, game);
                CheckGenres(gameDto.Genres, game);



                ////checking for genres 
                //foreach (var item in gameDto.Genres)
                //{
                //    var returnGenre = unitOfWork.GenreRepository.GetById(item);
                //    if (returnGenre == null)
                //    {
                //        throw new ArgumentException($"Invalid genre id: {item}");
                //    }

                //    unitOfWork.GameGenreRepository.Create(new GenreGame() { Game = game, Genre = returnGenre });
                //}


                unitOfWork.GameRepository.Create(game);
                await unitOfWork.CommitAsync();

            }
        }


        /// <summary>
        /// Check for Genres
        /// </summary>
        /// <param name="platforms"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        private void CheckGenres(List<int> genres, Game game)
        {
            //checking for genres 
            foreach (var item in genres)
            {
                var returnGenre = unitOfWork.GenreRepository.GetById(item);
                if (returnGenre == null)
                {
                    throw new ArgumentException($"Invalid genre id: {item}");
                }
                unitOfWork.GameGenreRepository.Create(new GenreGame() { Game = game, Genre = returnGenre });
            }

        }


        /// <summary>
        /// Check for platforms
        /// </summary>
        /// <param name="platforms"></param>
        /// <returns></returns>
        private void CheckPlatforms(List<int> platforms, Game game)
        {
            foreach (var item in platforms)
            {
                var returnPlatform = unitOfWork.PlatformTypeRepository.GetById(item);
                if (returnPlatform == null)
                {
                    throw new ArgumentException($"Invalid platformtype id: {item}");
                }
                unitOfWork.GamePlatformTypeRepository.Create(new GamePlatformType() { Game = game, PlatformType = returnPlatform });
            }
        }





        public async Task DeleteGame(int id)
        {
            using (unitOfWork)
            {
                var gameEntity = unitOfWork.GameRepository
                    .GetSingle(filter: g => g.Id == id);
                if (gameEntity == null)
                {
                    throw new ArgumentException("Invalid game id");
                }
                unitOfWork.GameRepository.Delete(gameEntity);
                await unitOfWork.CommitAsync();
            }
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
