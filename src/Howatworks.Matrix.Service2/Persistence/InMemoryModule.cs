using Autofac;
using Howatworks.Matrix.Core.InMemory;
using Howatworks.Matrix.Core.Repositories;

namespace Howatworks.Matrix.Service.Persistence
{
    public class InMemoryModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(InMemoryDbContext<>)).As(typeof(IDbContext<>)).SingleInstance();
        }

    }
}
