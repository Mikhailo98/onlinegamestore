using Autofac;
using AutoMapper;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;

namespace BusinessLogicLayer.Configuration
{
    public class DependencyServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            // builder.RegisterAssemblyTypes(this.GetType().Assembly).AsImplementedInterfaces();

            builder.RegisterType<CommentService>().As<ICommentService>();
         //   builder.RegisterType<GamePlatformService>().As<IGamePlatformService>();
            builder.RegisterType<GameService>().As<IGameService>();
            builder.RegisterType<GenreService>().As<IGenreService>();
            builder.RegisterType<PublisherService>().As<IPublisherService>();

           builder.RegisterModule(new DataAccessLayer.DependencyServiceModule());

        }

    }

}
