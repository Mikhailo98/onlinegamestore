using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class CreateAnswerModel
    {
        [Required]
        [MaxLength(250)]
        public string Body { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int GameId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
