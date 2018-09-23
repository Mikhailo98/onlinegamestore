using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class CreateCommentModel
    {
        [Required]
        [MaxLength(250)]
        public string Body { get; set; }

        [Required]
        public int GameId { get; set; }
        
    }
}
