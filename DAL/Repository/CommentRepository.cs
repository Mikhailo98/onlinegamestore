using Domain;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repository
{
    class CommentRepository : Repository<Comment, int>, ICommentRepository
    {

        public CommentRepository(ApplicationContext context) : base(context)
        {
        }
    }
}