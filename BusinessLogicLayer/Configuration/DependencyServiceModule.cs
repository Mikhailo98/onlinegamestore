using Autofac;
using AutoMapper;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DAL;
using Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogicLayer.Configuration
{
    public class DependencyServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
           // builder.RegisterType<IGameService>().As<GameService>();
            //builder.RegisterType<IPublisherService>().As<PublisherService>();
            //builder.RegisterType<IUnitOfWork>().As<UnitOfWork>();
            builder.RegisterAssemblyTypes(this.GetType().Assembly).AsImplementedInterfaces();
            builder.RegisterModule(new DataAccessLayer.DependencyServiceModule());




        }



    }

    public static class MyClass
    {
        public static void CustomDependencyResolver(this IServiceCollection service)
        {

          

            service.AddDbContext<ApplicationContext>();
            service.AddTransient<IUnitOfWork>(p => new UnitOfWork());

            service.AddTransient<IGameService>(p => new GameService(new UnitOfWork(), Mapper.Instance));
           // service.AddTransient<IPublisherService>(p => new PublisherService(new UnitOfWork()));

        }

        public static void CustomDependencyResolver(this IServiceCollection service, string connectionString)
        {
            //         service.AddTransient<IGameService>(p => new GameService(new UnitOfWork(new ApplicationContext(connectionString))));
        }
    }

}
