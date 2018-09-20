using Domain;
using Domain.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Repository
{
    class CommentRepository : Repository<Comment, int>, ICommentRepository
    {
        private ApplicationContext context;

        public CommentRepository(ApplicationContext context) : base(context)
        {
            this.context = context;
        }
    }
}