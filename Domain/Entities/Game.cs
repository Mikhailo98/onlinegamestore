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
        public decimal Price { get; set; }

        public int PublisherId { get; set; }
        public virtual Publisher Publisher { get; set; }

        public DateTime AddedToStore { get; set; } = DateTime.Now;
        
        public DateTime PublishDate { get; set; }


        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<GenreGame> GenreGames { get; set; }
        public virtual ICollection<GamePlatformType> GamePlatformTypes { get; set; }

        public Game()
        {
            Comments = new List<Comment>();
            GenreGames = new List<GenreGame>();
            GamePlatformTypes = new List<GamePlatformType>();
        }
    }
}
