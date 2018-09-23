using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Models.Dtos.CommentDto
{
    public class CreateCommentDto
    {
        public string Body { get; set; }

        public int GameId { get; set; }
    }
}
