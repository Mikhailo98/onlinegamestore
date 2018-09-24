using AutoMapper;
using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Models.Dtos.GameDto;
using BusinessLogicLayer.Models.Dtos.GenreDto;
using BusinessLogicLayer.Models.Dtos.PublisherDto;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
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
                     .ForPath(p => p.Publisher.Name, opt => opt.MapFrom(b => b.Publisher.Name))
                     .ForPath(p => p.Publisher.Id, opt => opt.MapFrom(b => b.Publisher.Id))
                     .ForPath(p => p.Publisher.Games, opt => opt.Ignore())
                     .ForPath(p => p.Platforms, opt => opt.MapFrom(j =>
                     j.GamePlatformTypes.Select(p => new PlatformDto()
                     {
                         Id = p.PlatformTypeId,
                         Type = p.PlatformType.Type,
                         
                     })))
                     .ForPath(p => p.Genres, opt => opt.MapFrom(j =>
                     j.GenreGames.Select(p => new GenreDto()
                     {
                         Id = p.GenreId,
                         Name = p.Genre.Name,
                     })));

            CreateMap<Game, EditGameDto>()
                .ForPath(p => p.Platforms, opt => opt.Ignore())
                .ForPath(p => p.Genres, opt => opt.Ignore());


            CreateMap<EditGameDto, Game>()
               .ForPath(p => p.Comments, opt => opt.Ignore())
               .ForPath(p => p.GamePlatformTypes, opt => opt.Ignore())
               .ForPath(p => p.GenreGames, opt => opt.Ignore())
               .ForPath(p => p.Publisher, opt => opt.Ignore());





            CreateMap<EditPublisherDto, Publisher>()
                .ForMember(p => p.Games, opt => opt.Ignore())
                .ForMember(p => p.Id, opt => opt.Ignore());







            CreateMap<CreateGameDto, Game>()
                .ForMember(p => p.GenreGames, opt => opt.Ignore());



            CreateMap<Publisher, PublisherDto>()
                .ForPath(p => p.Games, opt => opt.Ignore())
                .AfterMap((src, destin) =>
                {
                    List<GameDto> gamesList = new List<GameDto>();
                    foreach (var item in src.Games)
                    {
                        gamesList.Add(new GameDto
                        {
                            Id = item.Id,
                            Description = item.Description,
                            Name = item.Name,
                            Platforms = item.GamePlatformTypes.Select(p => new PlatformDto()
                            {
                                Id = p.PlatformTypeId,
                                Type = p.PlatformType.Type,
                            }).ToList(),
                            Genres = item.GenreGames.Select(p => new GenreDto()
                            {
                                Id = p.GenreId,
                                Name = p.Genre.Name
                            }).ToList(),
                        });
                    }
                    destin.Games = gamesList;
                });



            CreateMap<Comment, GenreDto>();

            CreateMap<PlatformDto, PlatformDto>();

            CreateMap<Game, DetailedGameModel>();

            CreateMap<EditGenreDto, Genre>()
                .ForMember(p => p.GenreGames, opt => opt.Ignore())
                .ForMember(p => p.HeadGenre, opt => opt.Ignore())
                .ForMember(p => p.SubGenres, opt => opt.Ignore());

            CreateMap<GenreDto, Genre>()
               .ForMember(p => p.SubGenres, opt => opt.Ignore());

            CreateMap<Genre, GenreDto>();



            CreateMap<GenreGame, GameDto>()
                .ForPath(p => p.Description, opt => opt.MapFrom(b => b.Game.Description))
                .ForPath(p => p.Id, opt => opt.MapFrom(b => b.Game.Id))
                .ForPath(p => p.Name, opt => opt.MapFrom(b => b.Game.Name));


            CreateMap<GenreGame, GenreDto>()
                .ForPath(p => p.Name, opt => opt.MapFrom(b => b.Genre.Name))
                .ForPath(p => p.Id, opt => opt.MapFrom(b => b.Genre.Id))
                .ForPath(p => p.Games, opt => opt.Ignore())
                .ForPath(p => p.SubGenres, opt => opt.Ignore())
                .ForPath(p => p.HeadGenreId, opt => opt.Ignore());

        }
    }
}
