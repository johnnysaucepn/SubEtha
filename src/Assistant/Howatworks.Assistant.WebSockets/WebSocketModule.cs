using Autofac;
using System.Reflection;

namespace Howatworks.Assistant.WebSockets
{
    public class WebSocketModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConnectionManager>().AsSelf().InstancePerDependency();

            foreach (var type in Assembly.GetExecutingAssembly().ExportedTypes)
            {
                if (type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
                {
                    builder.RegisterType(type).AsSelf().As<WebSocketHandler>().SingleInstance();
                }
            }
        }
    }
}
