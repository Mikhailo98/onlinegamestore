using Autofac;

namespace BusinessLogicLayer.Configuration
{
    public class DependencyServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterAssemblyTypes(this.GetType().Assembly).AsImplementedInterfaces();
            builder.RegisterModule(new DataAccessLayer.DependencyServiceModule());
        }

    }

}
