using AutoMapper;
using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Models;
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

            CreateMap<Game, GameDto>()
                 .ForMember(p => p.PublisherName, opt => opt.MapFrom(b => b.Publisher.Name));

            CreateMap<CreateGameDto, Game>()
                .ForMember(p => p.GenreGames, opt => opt.Ignore());

            CreateMap<Publisher, PublisherDto>();

            CreateMap<Comment, CommentDto>();

            CreateMap<PlatformDto, PlatformDto>();

            CreateMap<Game, DetailedGameModel>();
        }
    }
}
