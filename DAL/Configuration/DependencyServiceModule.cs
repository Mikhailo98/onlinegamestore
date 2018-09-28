using Autofac;
using DAL;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer
{
    public class DependencyServiceModule : Module
    {


        protected override void Load(ContainerBuilder builder)
        {
           // builder.RegisterAssemblyTypes(this.GetType().Assembly).AsImplementedInterfaces();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

        }

    }
}
