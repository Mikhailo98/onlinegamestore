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
            builder.RegisterAssemblyTypes(this.GetType().Assembly).AsImplementedInterfaces();
        }

    }
}
