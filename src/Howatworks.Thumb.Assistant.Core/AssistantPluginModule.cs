using Autofac;
using Howatworks.Thumb.Assistant.Core.ControlSimulators;

namespace Howatworks.Thumb.Assistant.Core
{
    public class AssistantPluginModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AssistantJournalProcessorPlugin>().AsSelf().SingleInstance();
            builder.RegisterType<StatusManager>().AsSelf().SingleInstance();
            builder.RegisterType<WebSocketConnectionManager>().AsSelf().SingleInstance();
            builder.RegisterType<GameControlBridge>().AsSelf().SingleInstance();
            builder.RegisterType<InputSimulatorKeyboardSimulator>().As<IVirtualKeyboardSimulator>().SingleInstance();
            builder.RegisterType<InputSimulatorMouseSimulator>().As<IVirtualMouseSimulator>().SingleInstance();
        }
    }
}
