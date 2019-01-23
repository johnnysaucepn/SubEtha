using Autofac;
using Microsoft.Extensions.Configuration;

namespace Thumb.Plugin.SubEtha
{
    public class SubEthaPluginModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var reader = c.Resolve<IConfiguration>();
                return new SubEthaJournalProcessorPlugin(reader.GetSection("Thumb.Shared"), reader.GetSection("Thumb.Plugin.SubEtha"));
            }).As<IJournalProcessorPlugin>().SingleInstance();

            //builder.Register<HttpUploadClient>().As<IUploader<>>().AsSingleInstance();
        }

    }
}
