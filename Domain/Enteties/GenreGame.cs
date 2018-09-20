using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class GenreGame
    {
        public int? GenreId { get; set; }
        public virtual Genre Genre { get; set; }

        public int? GameId { get; set; }
        public virtual Game Game { get; set; }
    }
}
