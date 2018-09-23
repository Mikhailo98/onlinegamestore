using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Dtos
{
    public class GameDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public PublisherDto Publisher { get; set; }
        public List<PlatformDto> Platforms { get; set; }
        public List<GenreDto> Genres { get; set; }
        public List<CommentDto> Comments { get; set; }
    }
}
