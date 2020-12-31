using Autofac;
using Howatworks.Assistant.Core.ControlSimulators;
using Howatworks.Assistant.WebSockets;

namespace Howatworks.Assistant.Core
{
    public class AssistantModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AssistantApp>().AsSelf().SingleInstance();
            builder.RegisterType<StatusManager>().AsSelf().SingleInstance();
            builder.RegisterType<ConnectionManager>().AsSelf().SingleInstance();
            builder.RegisterType<AssistantWebSocketHandler>().AsSelf().As<WebSocketHandler>().SingleInstance();
            builder.RegisterType<AssistantMessageProcessor>().AsSelf().SingleInstance();
            builder.RegisterType<GameControlBridge>().AsSelf().SingleInstance();
            builder.RegisterType<InputSimulatorKeyboardSimulator>().As<IVirtualKeyboardSimulator>().SingleInstance();
            builder.RegisterType<InputSimulatorMouseSimulator>().As<IVirtualMouseSimulator>().SingleInstance();
        }
    }
}
