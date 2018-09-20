using AutoMapper;
using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class PublisherService : IPublisherService
    {
        private IUnitOfWork unitOfWork;
        private IMapper mapper;
        

        public PublisherService(IUnitOfWork unit, IMapper mapper)
        {
            unitOfWork = unit;
            this.mapper = mapper; 
        }


        public PublisherService()
        {

        }


        public async Task CreatePublisher(PublisherDto newPublisher)
        {
            var publisher = unitOfWork.PublisherRepository.Get(p => p.Name == newPublisher.Name);
            if (publisher != null)
            {
                throw new ArgumentNullException("Publisher with such Name already exists");
            }

            unitOfWork.PublisherRepository.Create(new Publisher() { Name = newPublisher.Name });

            await unitOfWork.CommitAsync();
        }
    }
}
