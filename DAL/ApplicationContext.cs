using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.IO;
using System.Data;


namespace DAL
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }


        //maybe should deleted 
        public ApplicationContext() : base()
        {
            Database.EnsureCreated();




        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<PlatformType> PlatformTypes { get; set; }
        public DbSet<Publisher> Publishers { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=GamesStore;Trusted_Connection=True;");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //publisher
            modelBuilder.Entity<Publisher>()
               .HasIndex(p => p.Name)
               .IsUnique();


            //genre
            modelBuilder.Entity<Genre>()
             .HasIndex(p => p.Name)
             .IsUnique();

            //platformtype
             //   modelBuilder.Entity<PlatformType>()
             //.HasIndex(p => p.Name)
             //.IsUnique();


            //game-publisher
            modelBuilder.Entity<Game>()
               .HasOne(p => p.Publisher)
               .WithMany(b => b.Games);

            //comment-game
            modelBuilder.Entity<Comment>()
                .HasOne(p => p.Game)
                .WithMany(b => b.Comments);


            //genre
            modelBuilder.Entity<Genre>()
                .HasIndex(p => p.Name)
                .IsUnique();

            //genres-games
            modelBuilder.Entity<GenreGame>()
                .HasKey(p => new { p.GenreId, p.GameId });
            modelBuilder.Entity<GenreGame>()
                    .HasOne<Genre>(sc => sc.Genre)
                    .WithMany(s => s.GenreGames)
                    .HasForeignKey(sc => sc.GenreId);
            modelBuilder.Entity<GenreGame>()
                    .HasOne<Game>(sc => sc.Game)
                    .WithMany(s => s.GenreGames)
                    .HasForeignKey(sc => sc.GameId);

            //PlatformType
            modelBuilder.Entity<PlatformType>()
               .Property(p => p.Type)
               .IsRequired();


            //game-platformtype
            modelBuilder.Entity<GamePlatformType>()
           .HasKey(p => new { p.PlatformTypeId, p.GameId });
            modelBuilder.Entity<GamePlatformType>()
                    .HasOne<PlatformType>(sc => sc.PlatformType)
                    .WithMany(s => s.GamePlatformtypes)
                    .HasForeignKey(sc => sc.PlatformTypeId);
            modelBuilder.Entity<GamePlatformType>()
                    .HasOne<Game>(sc => sc.Game)
                    .WithMany(s => s.GamePlatformTypes)
                    .HasForeignKey(sc => sc.GameId);


        }
    }
}
