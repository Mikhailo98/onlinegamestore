using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Genre
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? HeadGenreId { get; set; }
        public virtual Genre HeadGenre { get; set; }

        public virtual ICollection<Genre> SubGenres { get; set; }
        public virtual ICollection<GenreGame> GenreGames { get; set; }
    }
}
