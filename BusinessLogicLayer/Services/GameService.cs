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
using Domain.Repository;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Models.Dtos.GameDto;

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


                var newGame = mapper.Map<Game>(gameDto);

                //#region New Check

                //var platforms = unitOfWork
                //                    .PlatformTypeRepository
                //                    .Get(p => gameDto.Platforms.Contains(p.Id))
                //                    .Select((pt) => new GamePlatformType()
                //                    {
                //                        Game = newGame,
                //                        PlatformType = pt
                //                    }).ToList();

                //var genres = unitOfWork
                //                    .GenreRepository
                //                    .Get(p => gameDto.Genres.Contains(p.Id))
                //                    .Select((gn) => new GenreGame()
                //                    {
                //                        Game = newGame,
                //                        Genre = gn

                //                    }).ToList();

                //if (platforms.Count != gameDto.Platforms.Count)
                //{
                //    throw new ArgumentException("Invalid platform Id");
                //}

                //if (genres.Count != gameDto.Genres.Count)
                //{
                //    throw new ArgumentException("Invalid genre Id");
                //}

                //#endregion

                //newGame.GamePlatformTypes = platforms;
                //newGame.GenreGames = genres;


                //    CheckPlatforms(gameDto.Platforms, newGame);
                //  CheckGenres(gameDto.Genres, newGame);
                unitOfWork.GameRepository.Create(newGame);

                await unitOfWork.CommitAsync();

            }
        }


        ///// <summary>
        ///// Check for Genres
        ///// </summary>
        ///// <param name="platforms"></param>
        ///// <param name="game"></param>
        ///// <returns></returns>
        //private void CheckGenres(List<int> genres, Game game)
        //{
        //    //checking for genres 
        //    foreach (var item in genres)
        //    {
        //        var returnGenre = unitOfWork.GenreRepository.GetSingle(i => i.Id == item);
        //        if (returnGenre == null)
        //        {
        //            throw new ArgumentException($"Invalid genre id: {item}");
        //        }
        //        unitOfWork.GameGenreRepository.Create(new GenreGame() { Game = game, Genre = returnGenre });
        //    }

        //}


        ///// <summary>
        ///// Check for platforms
        ///// </summary>
        ///// <param name="platforms"></param>
        ///// <returns></returns>
        //private void CheckPlatforms(List<int> platforms, Game game)
        //{
        //    foreach (var item in platforms)
        //    {
        //        var returnPlatform = unitOfWork.PlatformTypeRepository.GetSingle(i => i.Id == item);
        //        if (returnPlatform == null)
        //        {
        //            throw new ArgumentException($"Invalid platformtype id: {item}");
        //        }
        //        unitOfWork.GamePlatformTypeRepository.Create(new GamePlatformType() { Game = game, PlatformType = returnPlatform });
        //    }
        //}


        public async Task DeleteGame(int id)
        {
            using (unitOfWork)
            {
                var gameEntity = await unitOfWork.GameRepository.GetSingle(filter: g => g.Id == id);
                if (gameEntity == null)
                {
                    throw new ArgumentException("Invalid game id");
                }
                unitOfWork.GameRepository.Delete(gameEntity);
                await unitOfWork.CommitAsync();
            }
        }


        public async Task EditGame(EditGameDto editedGame)
        {
            using (unitOfWork)
            {
                var gameEntity = await unitOfWork.GameRepository
                .GetSingle(filter: g => g.Id == editedGame.Id);

                if (gameEntity == null)
                {
                    throw new ArgumentException("No such game found");
                }

                var game = mapper.Map<Game>(editedGame);

                unitOfWork.GameRepository.Update(gameEntity);
                await unitOfWork.CommitAsync();
            }
        }

        public async Task<DetailedGameModel> GetInfo(int id)
        {
            using (unitOfWork)
            {
                var gameEntity = await unitOfWork.GameRepository
                    .GetSingle(filter: g => g.Id == id);

                if (gameEntity == null)
                {
                    throw new ArgumentException("Invalid game id");
                }

                return new DetailedGameModel()
                {
                    Game = mapper.Map<GameDto>(gameEntity),
                    Comments = mapper.Map<List<CommentDto>>(gameEntity.Comments),
                    Platforms = mapper.Map<List<PlatformDto>>(gameEntity.GamePlatformTypes.Select(p => new PlatformDto()
                    {
                        Id = p.PlatformTypeId,
                        Type = p.PlatformType.Type
                    }))
                };
            };
        }


        public async Task<List<GameDto>> GetAll()
        {
            using (unitOfWork)
            {
                var games = await unitOfWork.GameRepository.Get();


                var gameDtos = mapper.Map<List<GameDto>>(games);
                return gameDtos;
            }

        }
    }
}
