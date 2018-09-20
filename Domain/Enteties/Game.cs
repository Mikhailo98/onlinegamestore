using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Game
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int PublisherId { get; set; }
        public virtual Publisher Publisher { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<GenreGame> GenreGames { get; set; }
        public virtual ICollection<GamePlatformType> GamePlatformTypes { get; set; }
    }
}
