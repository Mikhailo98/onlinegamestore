using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Dtos
{
    public class GenreCreateDto
    {
        public string Name { get; set; }
        public int? HeadGenreId { get; set; }

    }
}
