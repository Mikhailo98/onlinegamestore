using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Body { get; set; }

        public int? GameId { get; set; }
        public int? AnswerId { get; set; }
    }
}
