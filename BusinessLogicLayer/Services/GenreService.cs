using AutoMapper;
using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models.Dtos.GenreDto;
using Domain;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    class GenreService : IGenreService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GenreService(IUnitOfWork unit, IMapper mapper)
        {
            unitOfWork = unit;
            this.mapper = mapper;
        }




        public async Task AddGenre(GenreCreateDto genre)
        {
            using (unitOfWork)
            {
                if (await unitOfWork.GenreRepository.GetSingleAsync(g => g.Name == genre.Name) != null)
                {
                    throw new ArgumentException("Such Genre already exists");
                }

                Genre newGenre = new Genre() { Name = genre.Name };

                if (genre.HeadGenreId != null)
                {
                    Genre headGenre = await unitOfWork
                        .GenreRepository.GetSingleAsync(g => g.Id == genre.HeadGenreId);
                    if (headGenre == null)
                    {
                        throw new ArgumentException("Invalid Head Genre Id");
                    }
                    newGenre.HeadGenre = headGenre;
                }

                unitOfWork.GenreRepository.Create(newGenre);
                await unitOfWork.CommitAsync();

            }


        }

        public async Task DeleteGenre(int id)
        {
            using (unitOfWork)
            {
                var genreEntity = await unitOfWork.GenreRepository.GetSingleAsync(i => i.Id == id);
                if (genreEntity == null)
                {
                    throw new ArgumentException("Invalid Genre Id");
                }

                unitOfWork.GenreRepository.Delete(genreEntity);

                await unitOfWork.CommitAsync();
            }

        }

        public async Task EditGenre(EditGenreDto editedGenre)
        {
            using (unitOfWork)
            {
                var genreEntity = await unitOfWork.GenreRepository.GetSingleAsync(i => i.Id == editedGenre.Id);
                if (genreEntity == null)
                {
                    throw new ArgumentException("Invalid Genre Id");
                }


                GenreDto mapped = mapper.Map<GenreDto>(genreEntity);

                mapped.Name = editedGenre.Name;
                mapped.HeadGenreId = editedGenre.HeadGenreId;

                if (mapped.SubGenres.Any(p => p.HeadGenreId == mapped.Id))
                {
                    var subgenreEntity = await unitOfWork.GenreRepository.GetSingleAsync(i => i.HeadGenreId == mapped.Id);

                    GenreDto submapped = mapper.Map<GenreDto>(subgenreEntity);
                    submapped.HeadGenreId = null;

                    subgenreEntity = mapper.Map(submapped, subgenreEntity);
                    unitOfWork.GenreRepository.Update(subgenreEntity);

                }

                genreEntity = mapper.Map(mapped, genreEntity);

                unitOfWork.GenreRepository.Update(genreEntity);

                await unitOfWork.CommitAsync();
            }
        }


        //TODO: Automapper genregame configure(games don't show)
        public async Task<List<GenreDto>> GetAll()
        {
            using (unitOfWork)
            {
                var genreEntity = await unitOfWork.GenreRepository.GetAsync();
                var genres = mapper.Map<List<GenreDto>>(genreEntity);
                return genres;
            }
        }

        public async Task<GenreDto> GetInfo(int id)
        {
            using (unitOfWork)
            {
                var genreEntity = await unitOfWork.GenreRepository.GetSingleAsync(i => i.Id == id);
                if (genreEntity == null)
                {
                    throw new ArgumentException("Invalid Genre Id");
                }

                var genreDto = mapper.Map<GenreDto>(genreEntity);

                return genreDto;
            }

        }


        //TODO: automapper genres configure(games should show publihser and platforms)
        public async Task<List<GameDto>> GetGamesOfGenre(int id)
        {

            using (unitOfWork)
            {
                var genreEntity = await unitOfWork.GenreRepository.GetSingleAsync(p => p.Id == id);
                if (genreEntity == null)
                {
                    throw new ArgumentException("Invalid Genre Id");
                }

                var games = mapper.Map<List<GameDto>>(genreEntity.GenreGames);
                return games;
            }

        }
    }
}
