﻿using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using Domain.Repository;
using DAL.Repository;
using System.Threading.Tasks;
using DataAccessLayer.Repository;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {

        private ApplicationContext context = new ApplicationContext();

        private GameRepository gameRepository;
        private CommentRepository commentRepository;
        private GenreRepository genreRepository;
        private PublisherRepository publisherRepository;
        private PlatformTypeRepository platformTypeRepository;
        private GameGenreRepository gameGenreRepository;
        private GamePlatformTypeRepository gamePlatformTypeRepository;


        public IRepository<GamePlatformType> GamePlatformTypeRepository
        {
            get
            {
                return gamePlatformTypeRepository = gamePlatformTypeRepository ?? new GamePlatformTypeRepository(context);
            }
        }

        public IRepository<GenreGame> GameGenreRepository
        {
            get
            {
                return gameGenreRepository = gameGenreRepository ?? new GameGenreRepository(context);
            }
        }

        public IRepository<Game> GameRepository
        {
            get
            {
                return gameRepository = gameRepository ?? new GameRepository(context);
            }
        }

        public IRepository<Comment> CommentRepository
        {
            get
            {
                return commentRepository = commentRepository ?? new CommentRepository(context);
            }
        }

        public IRepository<Genre> GenreRepository
        {
            get
            {
                return genreRepository = genreRepository ?? new GenreRepository(context);
            }
        }

        public IRepository<Publisher> PublisherRepository
        {
            get
            {
                return publisherRepository = publisherRepository ?? new PublisherRepository(context);
            }
        }

        public IRepository<PlatformType> PlatformTypeRepository
        {
            get
            {
                return platformTypeRepository = platformTypeRepository ?? new PlatformTypeRepository(context);
            }
        }




        public void Commit()
        {
            context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await context.SaveChangesAsync();
        }




        #region Dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
