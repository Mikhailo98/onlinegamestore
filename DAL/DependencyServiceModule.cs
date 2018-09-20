using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public class DependencyServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // builder.RegisterType<IGameService>().As<GameService>();
            //builder.RegisterType<IPublisherService>().As<PublisherService>();
            //builder.RegisterType<IUnitOfWork>().As<UnitOfWork>();
            builder.RegisterAssemblyTypes(this.GetType().Assembly).AsImplementedInterfaces();
        }

    }
}
