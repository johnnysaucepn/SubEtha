using Autofac;
using Thumb.Plugin.Controller.ControlSimulators;

namespace Thumb.Plugin.Controller
{
    public class ControllerPluginModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ControllerJournalProcessorPlugin>().As<IJournalProcessorPlugin>().SingleInstance();
            builder.RegisterType<StatusManager>().AsSelf().SingleInstance();
            builder.RegisterType<WebSocketConnectionManager>().AsSelf().SingleInstance();
            builder.RegisterType<GameControlBridge>().AsSelf().SingleInstance();
            builder.RegisterType<InputSimulatorKeyboardSimulator>().As<IKeyboardSimulator>().SingleInstance();
            builder.RegisterType<NullMouseSimulator>().As<IMouseSimulator>().SingleInstance();
        }
    }
}
