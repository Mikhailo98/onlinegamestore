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

        public ApplicationContext() : base()
        {
            //  Database.EnsureDeleted();
        }


        static ApplicationContext()
        {

            // Initialize();
        }

        public static void Initialize()
        {
            using (var ctx = new ApplicationContext())
            {

                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();

                var publisher1 = new Publisher { Name = "Blizzard" };
                var publisher2 = new Publisher { Name = "Ubisoft" };


                var game1 = new Game
                {
                    Name = "StarCraft 2",
                    Description = "An awesome game",
                    Publisher = publisher1,
                    PublishDate = new DateTime(2015, 7, 20),
                    AddedToStore = DateTime.Now,
                    Price = 123.50m

                };

                var game2 = new Game()
                {
                    Name = "Far Cry 5",
                    Description = "An awesome game",
                    Publisher = publisher2,
                    PublishDate = new DateTime(2018, 7, 20),
                    AddedToStore = DateTime.Now,
                    Price = 55.50m

                };

                var game3 = new Game()
                {
                    Name = "Far Cry 4",
                    Description = "An awesome game",
                    Publisher = publisher2,
                    PublishDate = new DateTime(2017, 7, 20),
                    AddedToStore = DateTime.Now,
                    Price = 44.40m


                };
                var game4 = new Game()
                {
                    Name = "Far Cry 3",
                    Description = "An awesome game",
                    Publisher = publisher2,
                    PublishDate = new DateTime(2016, 7, 20),
                    AddedToStore = DateTime.Now,
                    Price = 33.30m

                };
                var game5 = new Game()
                {
                    Name = "Far Cry 2",
                    Description = "An awesome game",
                    Publisher = publisher2,
                    PublishDate = new DateTime(2015, 7, 20),
                    AddedToStore = DateTime.Now,
                    Price = 22.50m

                };



                var genre1 = new Genre { Name = "Strategy" };
                var genre2 = new Genre { Name = "RTS", HeadGenre = genre1 };
                var genre3 = new Genre { Name = "First - person shooter" };


                var gameGenre1 = new GenreGame() { Game = game1, Genre = genre1 };
                var gameGenre2 = new GenreGame() { Game = game1, Genre = genre2 };
                var gameGenre3 = new GenreGame() { Game = game2, Genre = genre3 };
                var gameGenre4 = new GenreGame() { Game = game3, Genre = genre3 };
                var gameGenre5 = new GenreGame() { Game = game4, Genre = genre3 };
                var gameGenre6 = new GenreGame() { Game = game5, Genre = genre3 };


                var platformtype1 = new PlatformType()
                {
                    Type = "desktop",
                    GamePlatformtypes = new List<GamePlatformType>()
                   {
                       new GamePlatformType() { Game = game2 },
                       new GamePlatformType() { Game = game3 },
                       new GamePlatformType() { Game = game4 },
                       new GamePlatformType() { Game = game5 },

                   }
                };
                var platformtype2 = new PlatformType()
                {
                    Type = "mobile",
                    GamePlatformtypes = new List<GamePlatformType>()
                   {
                       new GamePlatformType() { Game = game1 }
                   }
                };
                var platformtype3 = new PlatformType()
                {
                    Type = "console",
                    GamePlatformtypes = new List<GamePlatformType>()
                   {
                       new GamePlatformType() { Game = game1 }
                   }
                };
                var platformtype4 = new PlatformType()
                {
                    Type = "browser",
                    GamePlatformtypes = new List<GamePlatformType>()
                   {
                       new GamePlatformType() { Game = game1 }
                   }
                };

                ctx.Add(gameGenre1);
                ctx.Add(gameGenre2);
                ctx.Add(gameGenre3);
                ctx.Add(gameGenre4);
                ctx.Add(gameGenre5);
                ctx.Add(gameGenre6);



                game1.GenreGames = new List<GenreGame> { gameGenre1, gameGenre2 };

                ctx.Publishers.Add(publisher1);
                ctx.Publishers.Add(publisher2);

                ctx.Games.Add(game1);
                ctx.Games.Add(game2);
                ctx.Games.Add(game3);
                ctx.Games.Add(game4);
                ctx.Games.Add(game5);

                ctx.Genres.Add(genre1);
                ctx.Genres.Add(genre2);
                ctx.Genres.Add(genre3);

                var comment1 = new Comment() { Game = game1, Body = "First One" };
                var comment2 = new Comment() { Game = game1, Body = "Second", ParentComment = comment1 };
                var comment3 = new Comment() { Game = game1, Body = "Third", ParentComment = comment2 };
                var comment22 = new Comment() { Game = game1, Body = "Third", ParentComment = comment1 };


                ctx.Comments.Add(comment1);
                ctx.Comments.Add(comment2);
                ctx.Comments.Add(comment3);
                ctx.Comments.Add(comment22);



                ctx.PlatformTypes.Add(platformtype1);
                ctx.PlatformTypes.Add(platformtype2);
                ctx.PlatformTypes.Add(platformtype3);
                ctx.PlatformTypes.Add(platformtype4);


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
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=GamesStore;Trusted_Connection=True;");

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


            //Genre-subgenres-> cascade deleting
            modelBuilder.Entity<Genre>()
                .HasMany(p => p.SubGenres)
                .WithOne(b => b.HeadGenre)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);


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

            base.OnModelCreating(modelBuilder);

        }
    }
}
