using BusinessLogicLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Models
{
    public class DetailedGameModel
    {
        public GameDto Game { get; set; }

        public List<CommentDto> Comments { get; set; }
        public List<PlatformDto> Platforms { get; set; }
        public PublisherDto Publisher { get; set; }

    }
}
