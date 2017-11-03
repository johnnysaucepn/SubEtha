using Autofac;
using Howatworks.Configuration;

namespace Thumb.Plugin.SubEtha
{
    public class SubEthaPluginModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var reader = c.Resolve<IConfigLoader>();
                return new SubEthaJournalProcessorPlugin(reader.GetConfigurationSection("Thumb.Shared"), reader.GetConfigurationSection("Thumb.Plugin.SubEtha"));
            }).As<IJournalProcessorPlugin>().SingleInstance();

            //builder.Register<HttpUploadClient>().As<IUploader<>>().AsSingleInstance();
        }
        
    }
}
