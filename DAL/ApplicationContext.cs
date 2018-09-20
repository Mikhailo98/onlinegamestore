using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.IO;
using System.Data;
using System.Collections.Generic;

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
        }


        static ApplicationContext()
        {
            Initialize();
        }

        public static void Initialize()
        {
            using (var ctx = new ApplicationContext())
            {
                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();

                var publisher1 = new Publisher { Name = "Blizzard" };

                var game1 = new Game { Name = "StarCraft 2", Description = "An awesome game", Publisher = publisher1 };

                var genre1 = new Genre { Name = "Strategy" };
                var genre2 = new Genre { Name = "RTS" };

                var gameGenre1 = new GenreGame() { Game = game1, Genre = genre1 };
                var gameGenre2 = new GenreGame() { Game = game1, Genre = genre2 };

                game1.GenreGames = new List<GenreGame> { gameGenre1, gameGenre2 };

                ctx.Publishers.Add(publisher1);

                ctx.Games.Add(game1);

                ctx.Genres.Add(genre1);
                ctx.Genres.Add(genre2);



                ctx.PlatformTypes.Add(new PlatformType() { Type = "desktop" });
                ctx.PlatformTypes.Add(new PlatformType() { Type = "mobile" });
                ctx.PlatformTypes.Add(new PlatformType() { Type = "console" });
                ctx.PlatformTypes.Add(new PlatformType() { Type = "browser" });

                ctx.SaveChanges();
            }
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
            modelBuilder.Entity<PlatformType>()
             .HasIndex(p => p.Type)
             .IsUnique();


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
