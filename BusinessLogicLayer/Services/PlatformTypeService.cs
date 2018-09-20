using AutoMapper;
using BusinessLogicLayer.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    class PlatformTypeService : IPlatformTypeService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;


        public PlatformTypeService(IUnitOfWork unit, IMapper mapper)
        {
            unitOfWork = unit;
            this.mapper = mapper;
        }

        public Task AddPlatform(PlatformTypeCreate platform)
        {
            throw new NotImplementedException();
        }

        public Task DeletePlatform(int id)
        {
            throw new NotImplementedException();
        }

        public Task EditPlatform(int id, PlatformTypeDto editedGame)
        {
            throw new NotImplementedException();
        }

        public Task<PlatformTypeDto> GetInfo(int id)
        {
            throw new NotImplementedException();
        }
    }
}
