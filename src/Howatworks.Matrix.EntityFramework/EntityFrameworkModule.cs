/*using Autofac;
using Howatworks.Matrix.Core.Repositories;

namespace Howatworks.Matrix.EntityFramework
{
    public class EntityFrameworkModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EntityFrameworkLocationEntityRepository>().As<ILocationEntityRepository>().SingleInstance();
            builder.RegisterType<EntityFrameworkSessionEntityRepository>().As<ISessionEntityRepository>().SingleInstance();
            builder.RegisterType<EntityFrameworkShipEntityRepository>().As<IShipEntityRepository>().SingleInstance();
            builder.RegisterType<EntityFrameworkGroupRepository>().As<IGroupRepository>().SingleInstance();
        }
    }
}
*/