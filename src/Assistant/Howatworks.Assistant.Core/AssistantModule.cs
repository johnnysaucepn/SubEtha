using Autofac;
using Howatworks.Assistant.Core.ControlSimulators;
using Howatworks.SubEtha.Bindings;
using log4net;
using Microsoft.Extensions.Configuration;

namespace Howatworks.Assistant.Core
{
    public class AssistantModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
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
