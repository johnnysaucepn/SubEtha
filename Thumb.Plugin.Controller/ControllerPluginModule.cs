using Autofac;
using Howatworks.Configuration;

namespace Thumb.Plugin.Controller
{
    public class ControllerPluginModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var reader = c.Resolve<IConfigLoader>();
                return new ControllerJournalProcessorPlugin(reader.GetConfigurationSection("Thumb.Shared"), reader.GetConfigurationSection("Thumb.Plugin.SubEtha"));
            }).As<IJournalProcessorPlugin>().SingleInstance();
        }
        
    }
}
