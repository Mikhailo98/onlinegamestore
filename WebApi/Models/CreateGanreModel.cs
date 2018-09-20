using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class CreateGanreModel
    {
        [Required]
        public string Name { get; set; }

        public int? HeadGenre { get; set; }
    }
}