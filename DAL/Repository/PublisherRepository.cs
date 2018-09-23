using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Repository;
using Domain;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    internal class PublisherRepository : IRepository<Publisher>
    {

        private readonly ApplicationContext context;
        private readonly DbSet<Publisher> dbSet;


        public PublisherRepository(ApplicationContext context)
        {
            this.context = context;
            dbSet = context.Set<Publisher>();

        }

        public async Task<List<Publisher>> GetAsync()
        {
            return await dbSet.ToListAsync();
        }
                     
        public void Create(Publisher entity)
        {
            dbSet.Add(entity);
        }

        public void Update(Publisher entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(Publisher entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public async Task<Publisher> GetSingleAsync(Expression<Func<Publisher, bool>> filter)
        {
            IQueryable<Publisher> query = dbSet;

            return  await query
                .Include(p => p.Games)
                
                .FirstOrDefaultAsync(filter);

        }

        public async Task<IEnumerable<Publisher>> GetAsync(Expression<Func<Publisher, bool>> filter = null,
            Func<IQueryable<Publisher>, IOrderedQueryable<Publisher>> orderBy = null)
        {
            IQueryable<Publisher> query = dbSet;

            query = query
                  .Include(p => p.Games);

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
    }
}