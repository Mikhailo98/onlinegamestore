using AutoMapper;
using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Interfaces;
using Domain;
using Domain.Repository;
using System;
using System.Collections.Generic;
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
                if (unitOfWork.GenreRepository.GetSingle(g => g.Name == genre.Name) != null)
                {
                    throw new ArgumentException("Such Genre already exists");
                }


                Genre newGenre = new Genre() { Name = genre.Name };

                if (genre.HeadGenreId != null)
                {
                    Genre headGenre = unitOfWork.GenreRepository.GetSingle(g => g.Id == genre.HeadGenreId);

                    if (headGenre == null)
                    {
                        throw new ArgumentException("Invalid Head Genre Id");
                    }

                    newGenre.HeadGenre = headGenre;

                    headGenre.SubGenres.Add(newGenre);
                    unitOfWork.GenreRepository.Update(headGenre);
                }


                unitOfWork.GenreRepository.Create(newGenre);
                await unitOfWork.CommitAsync();

            }


        }

        public async Task DeleteGenre(int id)
        {
            using (unitOfWork)
            {
                var genreEntity = unitOfWork.GenreRepository.GetById(id);
                if (genreEntity == null)
                {
                    throw new ArgumentException("Invalid Genre Id");
                }

                unitOfWork.GenreRepository.Delete(genreEntity);

                await unitOfWork.CommitAsync();
            }

        }

        public Task EditGenre(int id, GenreDto editedGenre)
        {
            throw new NotImplementedException();
        }

        public async Task<GenreDto> GetInfo(int id)
        {
            using (unitOfWork)
            {
                var genreEntity = ((IGenreRepository)unitOfWork.GenreRepository).GetGenreFullInfo(id);
                if (genreEntity == null)
                {
                    throw new ArgumentException("Invalid Genre Id");
                }

                var v = mapper.Map<GenreDto>(genreEntity);

                return v;
            }

        }
    }
}
