using Autofac;
using OnionDemo.Domain.Services.Abstractions;

namespace OnionDemo.Services
{
    public class ServiceAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ServiceManager>().As<IServiceManager>();
            builder.RegisterType<PingPongService>().As<IPingPongService>();

            // Other Lifetime
            // Transient
            //builder.RegisterType<PingPongService>().As<IPingPongService>()
            //    .InstancePerDependency();

            //// Scoped
            //builder.RegisterType<PingPongService>().As<IPingPongService>()
            //    .InstancePerLifetimeScope();

            //builder.RegisterType<PingPongService>().As<IPingPongService>()
            //    .InstancePerRequest();

            //// Singleton
            //builder.RegisterType<PingPongService>().As<IPingPongService>()
            //    .SingleInstance();

            // Scan an assembly for components
            //builder.RegisterAssemblyTypes(typeof(AssemblyReference).Assembly)
            //       .Where(t => t.Name.EndsWith("Service"))
            //       .AsImplementedInterfaces();
        }
    }
}
