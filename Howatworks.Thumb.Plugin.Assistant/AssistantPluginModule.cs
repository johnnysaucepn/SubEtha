using Autofac;
using Howatworks.Thumb.Plugin.Assistant.ControlSimulators;

namespace Howatworks.Thumb.Plugin.Assistant
{
    public class AssistantPluginModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AssistantJournalProcessorPlugin>().As<IJournalProcessorPlugin>().SingleInstance();
            builder.RegisterType<StatusManager>().AsSelf().SingleInstance();
            builder.RegisterType<WebSocketConnectionManager>().AsSelf().SingleInstance();
            builder.RegisterType<GameControlBridge>().AsSelf().SingleInstance();
            builder.RegisterType<InputSimulatorKeyboardSimulator>().As<IVirtualKeyboardSimulator>().SingleInstance();
            builder.RegisterType<InputSimulatorMouseSimulator>().As<IVirtualMouseSimulator>().SingleInstance();
        }
    }
}
