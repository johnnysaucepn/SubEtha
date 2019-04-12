using Autofac;

namespace Thumb.Plugin.SubEtha
{
    public class SubEthaPluginModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpUploadClient>().AsSelf().SingleInstance();

            builder.RegisterType<SubEthaJournalProcessorPlugin>().As<IJournalProcessorPlugin>().SingleInstance();
        }
    }
}
