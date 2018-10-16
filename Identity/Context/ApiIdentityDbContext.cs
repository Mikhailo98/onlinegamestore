using Identity.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Context
{
    public class AppIdentityDbContext : IdentityDbContext<User>
    {

        static AppIdentityDbContext()
        {
            Initialize();
        }

        private static void Initialize()
        {
            using (var ctx = new AppIdentityDbContext())
            {
                //ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();

            }
        }

        public AppIdentityDbContext() : base()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=GamesStore_Identity;Trusted_Connection=True;");

        }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }

    }
}
