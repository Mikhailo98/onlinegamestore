using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Models.Dtos.GenreDto
{
    public class EditGenreDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? HeadGenreId { get; set; }
    }
}
