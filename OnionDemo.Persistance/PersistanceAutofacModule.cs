
using Autofac;
using OnionDemo.Domain.Repositories;
using OnionDemo.Persistance.Repositories;

namespace OnionDemo.Presentation
{
    public class PersistanceAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RepositoryManager>().As<IRepositoryManager>();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<PingPongRepository>().As<IPingPongRepository>();

            //builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();

            // Other Lifetime
            // Transient
            //builder.RegisterType<PingPongRepository>().As<IPingPongRepository>()
            //    .InstancePerDependency();

            //// Scoped
            //builder.RegisterType<PingPongRepository>().As<IPingPongRepository>()
            //    .InstancePerLifetimeScope();

            //builder.RegisterType<PingPongRepository>().As<IPingPongRepository>()
            //    .InstancePerRequest();

            //// Singleton
            //builder.RegisterType<PingPongRepository>().As<IPingPongRepository>()
            //    .SingleInstance();

            // Scan an assembly for components
            //builder.RegisterAssemblyTypes(typeof(AssemblyReference).Assembly)
            //       .Where(t => t.Name.EndsWith("Repository"))
            //       .AsImplementedInterfaces();
        }
    }
}
