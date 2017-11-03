using Autofac;
using SubEtha.Core.InMemory;
using SubEtha.Core.Repositories;

namespace SubEtha.Service.Persistence
{
    public class InMemoryModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(InMemoryDbContext<>)).As(typeof(IDbContext<>)).SingleInstance();
        }

    }
}
