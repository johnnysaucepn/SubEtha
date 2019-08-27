using Autofac;
using Howatworks.Thumb.Assistant.Core.ControlSimulators;

namespace Howatworks.Thumb.Assistant.Core
{
    public class AssistantModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AssistantApp>().AsSelf().SingleInstance();
            builder.RegisterType<StatusManager>().AsSelf().SingleInstance();
            builder.RegisterType<WebSocketConnectionManager>().AsSelf().SingleInstance();
            builder.RegisterType<GameControlBridge>().AsSelf().SingleInstance();
            builder.RegisterType<InputSimulatorKeyboardSimulator>().As<IVirtualKeyboardSimulator>().SingleInstance();
            builder.RegisterType<InputSimulatorMouseSimulator>().As<IVirtualMouseSimulator>().SingleInstance();
        }
    }
}
