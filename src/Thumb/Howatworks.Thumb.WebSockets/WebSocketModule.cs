using Autofac;

namespace Howatworks.Thumb.WebSockets
{
    public class WebSocketModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConnectionManager>().AsSelf().InstancePerDependency();
        }
    }
}
