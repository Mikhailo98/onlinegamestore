using AutoMapper;
using BusinessLogicLayer.Dtos;
using BusinessLogicLayer.Models.Dtos.PublisherDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<CreateCommentModel, CreateAnswerCommentDto>()
                .ForMember(p => p.AnswerId, opt => opt.Ignore());

                cfg.CreateMap<PublisherCreateModel, EditPublisherDto>()
                            .ForPath(p => p.Id, opt => opt.Ignore());

                cfg.CreateMap<CreateAnswerCommentDto, CreateCommentModel>();

                cfg.AddProfile(new BusinessLogicLayer.Configuration.AutoMapperConfig());
            });
        }
    }
}
