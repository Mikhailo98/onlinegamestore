using AutoMapper;
using BusinessLogicLayer.Dtos;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Configuration
{
    public class AutoMapperConfig : Profile
    {

        public AutoMapperConfig()
        {
            CreateMap<GameDto, Game>();

            CreateMap<Game, GameDto>();

        }
    }
}
