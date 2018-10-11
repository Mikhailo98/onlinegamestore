using BusinessLogicLayer.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BusinessLogicLayer.Dtos;
using AutoMapper;
using BusinessLogicLayer.Models.Dtos.GameDto;
using BusinessLogicLayer.Models.Dtos.CommentDto;
using System.Runtime.CompilerServices;
using BusinessLogicLayer.Pagination;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Domain.Filter;
using DataAccessLayer.Filter;
using SortDropDownList = BusinessLogicLayer.Pagination.SortDropDownList;

[assembly: InternalsVisibleTo("BusinessLogicLayer.Test")]

namespace BusinessLogicLayer.Services
{
    class GameService : IGameService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<GameService> logger;
        private readonly IGameFilter filter;

        public GameService(IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<GameService> logger, IGameFilter filter)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
            this.filter = filter;
        }


        public async Task AddGame(CreateGameDto gameDto)
        {
            using (unitOfWork)
            {
                if (await unitOfWork.GameRepository.GetSingleAsync(g =>
                g.Name == gameDto.Name && g.PublisherId == gameDto.PublisherId) != null)
                {
                    throw new ArgumentException("Game with such name of such publisher already exists");
                }

                if (await unitOfWork.PublisherRepository.GetSingleAsync(p => p.Id == gameDto.PublisherId) == null)
                {
                    throw new ArgumentException("invalid publisher id");
                }


                var newGame = mapper.Map<Game>(gameDto);

                unitOfWork.GameRepository.Create(newGame);

                await AddPlatforms(gameDto.Platforms, newGame);
                await AddGenres(gameDto.Genres, newGame);

                await unitOfWork.CommitAsync();

            }
        }


        /// <summary>
        /// Check for Genres
        /// </summary>
        /// <param name="platforms"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        private async Task AddGenres(List<int> genres, Game game)
        {
            //checking for genres 
            foreach (var item in genres)
            {
                var returnGenre = await unitOfWork.GenreRepository.GetSingleAsync(i => i.Id == item);
                if (returnGenre == null)
                {
                    throw new ArgumentException($"Invalid genre id: {item}", nameof(returnGenre));
                }
                unitOfWork.GameGenreRepository.Create(new GenreGame() { Game = game, Genre = returnGenre });
            }

        }


        /// <summary>
        /// Check for platforms
        /// </summary>
        /// <param name="platforms"></param>
        /// <returns></returns>
        private async Task AddPlatforms(List<int> platforms, Game game)
        {
            foreach (var item in platforms)
            {
                var returnPlatform = await unitOfWork.PlatformTypeRepository.GetSingleAsync(i => i.Id == item);
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
                var gameEntity = await unitOfWork.GameRepository.GetSingleAsync(filter: g => g.Id == id);
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
                Game gameEntity = await unitOfWork.GameRepository
                .GetSingleAsync(filter: g => g.Id == editedGame.Id);

                if (gameEntity == null)
                {
                    throw new ArgumentException("No such game found");
                }

                //mapping real entity
                EditGameDto mappedGameEnity = mapper.Map<EditGameDto>(gameEntity);

                //changing values of Mapped Entity
                mappedGameEnity = mapper.Map(editedGame, mappedGameEnity);


                //mapping back to real entity
                gameEntity = mapper.Map(editedGame, gameEntity);

                await DeletePlatforms(editedGame.Platforms, gameEntity);
                await DeleteGenres(editedGame.Genres, gameEntity);

                unitOfWork.GameRepository.Update(gameEntity);

                await unitOfWork.CommitAsync();
            }
        }

        #region Edit Game private methods

        private async Task DeletePlatforms(List<int> platformsId, Game entity)
        {
            foreach (var platform in entity.GamePlatformTypes)
            {
                unitOfWork.GamePlatformTypeRepository.Delete(platform);
            }

            await AddPlatforms(platformsId, entity);
        }

        private async Task DeleteGenres(List<int> genreIds, Game entity)
        {
            foreach (var genre in entity.GenreGames)
            {
                unitOfWork.GameGenreRepository.Delete(genre);
            }

            await AddGenres(genreIds, entity);
        }



        #endregion

        public async Task<GameDto> GetInfo(int id)
        {
            using (unitOfWork)
            {
                logger.LogInformation($"Request with id {id}");
                var gameEntity = await unitOfWork.GameRepository
                    .GetSingleAsync(filter: g => g.Id == id);

                if (gameEntity == null)
                {
                    throw new ArgumentException("Invalid game id");

                }

                var dto = mapper.Map<GameDto>(gameEntity);
                dto.Comments = dto.Comments.Where(p => p.ParentCommentId == null)?.ToList();
                return dto;
            };
        }


        public async Task<List<GameDto>> GetAll()
        {
            using (unitOfWork)
            {
                var games = await unitOfWork.GameRepository.GetAsync();

                var gameDtos = mapper.Map<List<GameDto>>(games);
                return gameDtos;
            }

        }

        public async Task<List<CommentDto>> GetAllComments(int id)
        {
            using (unitOfWork)
            {
                var gameEntity = await unitOfWork.GameRepository.GetSingleAsync(c => c.Id == id);
                var comments = mapper.Map<List<CommentDto>>(gameEntity.Comments);
                return comments;
            }

        }

        public async Task<List<GenreDto>> GetGenres(int id)
        {
            using (unitOfWork)
            {
                var gamesEntity = await unitOfWork.GameRepository.GetSingleAsync(c => c.Id == id);
                var genres = mapper.Map<List<GenreDto>>(gamesEntity.GenreGames);
                return genres;
            }
        }

        public async Task CommentGame(CreateCommentDto comment)
        {
            using (unitOfWork)
            {
                var gameEntity = await unitOfWork.GameRepository.GetSingleAsync(p => p.Id == comment.GameId);
                if (gameEntity == null)
                {
                    throw new ArgumentException("Invalid game Id");
                }

                unitOfWork.CommentRepository.Create(new Comment()
                {
                    Name = comment.Name,
                    Game = gameEntity,
                    Body = comment.Body
                });

                await unitOfWork.CommitAsync();
            }
        }

        public async Task<string> GetGameLocalPath(int id)
        {
            logger.LogInformation($"Request with id {id}");
            var gameEntity = await unitOfWork.GameRepository.GetSingleAsync(p => p.Id == id);
            if (gameEntity == null)
            {
                throw new ArgumentException("Invalid game Id");
            }

            return "Files/book.pdf";
        }




        //TODO:
        public async Task<List<GameDto>> OrderedBy(PagingParamsBll paging)
        {
            //TODO: factory
            Func<IQueryable<Game>, IOrderedQueryable<Game>> orderby = (q) =>
            {
                IOrderedQueryable<Game> order;
                q = q.Skip(paging.PageSize * (paging.PageNumber - 1)).Take(paging.PageSize);

                switch (paging.DropdownList)
                {
                    case SortDropDownList.byPriceDesc:
                        order = q.OrderByDescending(j => j.Price);
                        break;
                    case SortDropDownList.ByPriceAsc:
                        order = q.OrderBy(j => j.Price);
                        break;
                    case SortDropDownList.MostCommented:
                        order = q.OrderByDescending(j => j.Comments.Count);
                        break;
                    case SortDropDownList.mostViewed:
                        order = q.OrderByDescending(j => j.Comments.Count);
                        break;
                    case SortDropDownList.New:
                        order = q.OrderByDescending(j => j.AddedToStore.Date);
                        break;
                    default:
                        order = null;
                        break;
                }
                return order;
            };


            var returnedFilter = filter
                            .IncludeGenres(paging.Genres)
                            .IncludePlatforms(paging.Platforms)
                            .SetMaxPrice(paging.MaxPrice)
                            .SetMinPrice(paging.MinPrice)
                            .FilterExpression;




            //Expression<Func<Game, bool>> expression = p =>
            //p.GenreGames.Select(s => s.GenreId).Intersect(paging.Genres).Count() == paging.Genres.Count &&
            //p.GamePlatformTypes.Select(s => s.PlatformTypeId).Intersect(paging.Platforms).Count() == paging.Platforms.Count &&
            //p.Price >= paging.MinPrice && p.Price <= paging.MaxPrice;

            IQueryable<Game> listOppLineData = Enumerable.Empty<Game>().AsQueryable();
            listOppLineData = listOppLineData.Skip(paging.PageSize * (paging.PageNumber - 1)).Take(paging.PageSize);

           
            var r = Context.DoContext(DataAccessLayer.Filter.SortType.ByPriceAsc,
                listOppLineData);


            var result = await unitOfWork.GameRepository
                .GetAsync(returnedFilter, r);

            var mappedResult = mapper.Map<List<GameDto>>(result);
            return mappedResult;

        }
    }


}
