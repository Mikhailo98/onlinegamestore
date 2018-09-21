using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Models.Dtos.GameDto
{
    public class EditGameDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PublisherId { get; set; }
        public List<int> Genres { get; set; }
        public List<int> Platforms { get; set; }
    }
}
