using Autofac;
using Howatworks.Assistant.Core.ControlSimulators;
using Howatworks.Assistant.WebSockets;
using Howatworks.SubEtha.Bindings;

namespace Howatworks.Assistant.Core
{
    public class AssistantModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<AssistantWebSocketModule>();

            builder.RegisterType<AssistantApp>().AsSelf().SingleInstance();
            builder.RegisterType<StatusManager>().AsSelf().SingleInstance();
            builder.RegisterType<AssistantMessageHub>().AsSelf().SingleInstance();
            builder.RegisterType<GameControlBridge>().AsSelf().SingleInstance();
            builder.RegisterType<InputSimulatorKeyboardSimulator>().As<IVirtualKeyboardSimulator>().SingleInstance();
            builder.RegisterType<InputSimulatorMouseSimulator>().As<IVirtualMouseSimulator>().SingleInstance();
            builder.RegisterType<DynamicBindingMapper>().As<IBindingMapper>().SingleInstance();
            builder.RegisterType<AssistantMessageParser>().AsSelf().SingleInstance();
        }
    }
}
