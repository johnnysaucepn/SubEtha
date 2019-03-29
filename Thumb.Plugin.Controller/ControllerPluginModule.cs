using Autofac;

namespace Thumb.Plugin.Controller
{
    public class ControllerPluginModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ControllerJournalProcessorPlugin>().As<IJournalProcessorPlugin>().SingleInstance();
            builder.RegisterType<StatusManager>().AsSelf().SingleInstance();
        }
    }
}
