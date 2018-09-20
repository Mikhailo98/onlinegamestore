using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repository
{
    public interface ICommentRepository : IRepository<Comment, int>
    {
    }
}
