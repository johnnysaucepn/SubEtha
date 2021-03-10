using Autofac;
using Howatworks.Thumb.WebSockets;

namespace Howatworks.Assistant.WebSockets
{
    public class AssistantWebSocketModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<WebSocketModule>();

            builder.RegisterType<AssistantWebSocketHandler>().AsSelf().As<WebSocketHandler>().SingleInstance();
        }
    }
}
