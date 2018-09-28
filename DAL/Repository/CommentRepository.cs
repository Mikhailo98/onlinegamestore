using Domain;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    class CommentRepository : IRepository<Comment>
    {
        private readonly ApplicationContext context;
        private readonly DbSet<Comment> dbSet;


        public CommentRepository(ApplicationContext context)
        {
            this.context = context;
            dbSet = context.Set<Comment>();
        }


        public void Create(Comment entity)
        {
            dbSet.Add(entity);
        }

        public void Delete(Comment entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

     
        public async Task<IEnumerable<Comment>> GetAsync(Expression<Func<Comment, bool>> filter = null, Func<IQueryable<Comment>, IOrderedQueryable<Comment>> orderBy = null)
        {
            IQueryable<Comment> query = dbSet;

            query = query
               .Include(p => p.ParentComment)
               .Include(p => p.Game);


            if (filter != null)
            {
                query = query.Where(filter);
            }


            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task<Comment> GetSingleAsync(Expression<Func<Comment, bool>> filter)
        {
            IQueryable<Comment> query = dbSet;

            return await query
             .Include(p => p.ParentComment)
           .Include(p => p.Game)
           .FirstOrDefaultAsync(filter);

        }

        public void Update(Comment entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
    }
}