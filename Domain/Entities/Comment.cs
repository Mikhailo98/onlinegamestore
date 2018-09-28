using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Comment
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Body { get; set; }

        public int GameId { get; set; }
        public virtual Game Game { get; set; }

        public int? ParentCommentId { get; set; }
        public virtual Comment ParentComment { get; set; }

        public virtual ICollection<Comment> ChildComments { get; set; }

        public Comment()
        {
            ChildComments = new List<Comment>();
        }
    }
}
