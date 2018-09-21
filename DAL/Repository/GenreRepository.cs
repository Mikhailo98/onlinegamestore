using DAL.Repository;
using Domain;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL
{
    internal class GenreRepository : IRepository<Genre>
    {
        private readonly ApplicationContext context;
        private readonly DbSet<Genre> dbSet;


        public GenreRepository(ApplicationContext context)
        {
            this.context = context;
            dbSet = context.Set<Genre>();
        }

        public void Create(Genre entity)
        {
            dbSet.Add(entity);
        }

        public void Delete(Genre entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            dbSet.Remove(entity);
        }

        public async Task<List<Genre>> Get()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<IEnumerable<Genre>> Get(Expression<Func<Genre, bool>> filter = null, Func<IQueryable<Genre>, IOrderedQueryable<Genre>> orderBy = null)
        {
            IQueryable<Genre> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter)
                    .Include(p => p.GenreGames)
                    .ThenInclude(p => p.Game)
                    .Include(p => p.HeadGenre)
                    .Include(p => p.SubGenres);
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



        public async Task<Genre> GetSingle(Expression<Func<Genre, bool>> filter)
        {
            IQueryable<Genre> query = dbSet;

            return await query
                 .Include(p => p.GenreGames)
                    .ThenInclude(p => p.Game)
                    .Include(p => p.HeadGenre)
                    .Include(p => p.SubGenres)
                .FirstOrDefaultAsync(filter);
        }

        public void Update(Genre entity)
        {
            dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
        }
    }

}