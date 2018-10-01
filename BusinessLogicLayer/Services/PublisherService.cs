using AutoMapper;
using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models.Dtos.PublisherDto;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;


        public PublisherService(IUnitOfWork unit, IMapper mapper)
        {
            unitOfWork = unit;
            this.mapper = mapper;
        }




        public async Task CreatePublisher(CreatePublisherDto newPublisher)
        {

            using (unitOfWork)
            {
                var publisher = unitOfWork.PublisherRepository.GetSingleAsync(p => p.Name == newPublisher.Name);
                if (await publisher != null)
                {
                    throw new ArgumentException("Publisher with such Name already exists");
                }

                unitOfWork.PublisherRepository.Create(new Publisher() { Name = newPublisher.Name });

                await unitOfWork.CommitAsync();
            }
        }

        public async Task EditPublisher(EditPublisherDto genreDto)
        {
            using (unitOfWork)
            {
                var publisher = await unitOfWork.PublisherRepository.GetSingleAsync(p => p.Id == genreDto.Id);
                if (publisher == null)
                {
                    throw new ArgumentException("Invalid publisher Id");
                }

                publisher = mapper.Map(genreDto, publisher);

                unitOfWork.PublisherRepository.Update(publisher);
                await unitOfWork.CommitAsync();
            }
        }

        public async Task<List<PublisherDto>> GetAll()
        {
            using (unitOfWork)
            {
                var publisherEntities = await unitOfWork.PublisherRepository.GetAsync();
                var dto = mapper.Map<List<PublisherDto>>(publisherEntities);
                return dto;
            }
        }

        public async Task<List<GameDto>> GetGamesOfPublisher(int id)
        {
            using (unitOfWork)
            {
                var gamesEnteties = await unitOfWork.GameRepository.GetAsync(p => p.PublisherId == id);
                var dto = mapper.Map<List<GameDto>>(gamesEnteties);
                return dto;
            }
        }

        public async Task<PublisherDto> GetInfo(int id)
        {
            using (unitOfWork)
            {
                var publisherEntity = await unitOfWork.PublisherRepository.GetSingleAsync(p => p.Id == id);
                var dto = mapper.Map<PublisherDto>(publisherEntity);
                return dto;
            }

        }
    }
}
