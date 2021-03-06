﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class EditGenreModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int? HeadGenreId { get; set; }
    }
}
