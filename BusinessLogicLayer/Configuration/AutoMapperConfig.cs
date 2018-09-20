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
            //source -> destination

            CreateMap<GameDto, Game>();

            CreateMap<Game, GameDto>();

            CreateMap<CreateGameDto, Game>()
                .ForMember(p => p.GenreGames, opt => opt.Ignore());

        }
    }
}
