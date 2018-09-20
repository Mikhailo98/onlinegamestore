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

        public string Description { get; set; }

        [Required]
        public int PublisherId { get; set; }
        
        [Required]
        public List<string> Genres { get; set; }

    }
}
