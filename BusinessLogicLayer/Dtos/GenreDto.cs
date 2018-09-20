using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Dtos
{
    public class GenreDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? HeadGenreId { get; set; }

        public List<GenreDto> SubGenres { get; set; }
        
        public List<GameDto> Games { get; set; }
    }
}
