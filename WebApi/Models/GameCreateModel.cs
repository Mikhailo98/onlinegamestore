using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class GameCreateModel
    {

        [Required]
        public string Name { get; set; }

        [MaxLength(400)]
        public string Description { get; set; }

        [Required]
        public int PublisherId { get; set; }

        [Required]
        public List<int> Genres { get; set; }

        [Required]
        public List<int> Platforms { get; set; }

    }
}
