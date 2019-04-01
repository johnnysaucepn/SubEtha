using Autofac;
using Howatworks.PlayerJournal.Parser;
using Microsoft.Extensions.Configuration;

namespace Thumb.Plugin.SubEtha
{
    public class SubEthaPluginModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpUploadClient>().AsSelf().SingleInstance();

            builder.Register(c =>
            {
                var reader = c.Resolve<IConfiguration>();
                var router = c.Resolve<JournalEntryRouter>();

                return new SubEthaJournalProcessorPlugin(reader.GetSection("Thumb.Shared"), reader.GetSection("Thumb.Plugin.SubEtha"), router);
            }).As<IJournalProcessorPlugin>().SingleInstance();


        }

    }
}
