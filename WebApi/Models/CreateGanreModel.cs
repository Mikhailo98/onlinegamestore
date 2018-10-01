using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class CreateGanreModel
    {
        [Required]
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int? HeadGenre { get; set; }
    }
}