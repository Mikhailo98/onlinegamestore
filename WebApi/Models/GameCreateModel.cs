using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Infrastucture;

namespace WebApi.Models
{
    public class GameCreateModel
    {

        [Required]
        public string Name { get; set; }

        [MaxLength(400)]
        public string Description { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int PublisherId { get; set; }

        [Required]
        [CustomIDsValidation]
        public List<int> Genres { get; set; }

        [Required]
        [CustomIDsValidation]
        public List<int> Platforms { get; set; }

    }
}
