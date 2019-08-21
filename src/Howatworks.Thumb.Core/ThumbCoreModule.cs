using Autofac;
using Howatworks.SubEtha.Monitor;
using Howatworks.SubEtha.Parser;
using Microsoft.Extensions.Configuration;

namespace Howatworks.Thumb.Core
{
    public class ThumbCoreModule : Module
    {
        private readonly IConfiguration _config;

        public ThumbCoreModule(IConfiguration config)
        {
            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => _config).As<IConfiguration>().SingleInstance();

            builder.RegisterType<ThumbApp>().SingleInstance();

            builder.RegisterType<JsonJournalMonitorState>().As<IJournalMonitorState>().SingleInstance();
            builder.RegisterType<JournalMonitorScheduler>().AsSelf().SingleInstance();
            builder.RegisterType<JournalParser>().As<IJournalParser>().SingleInstance();
            builder.RegisterType<JournalReaderFactory>().As<IJournalReaderFactory>().SingleInstance();
            builder.RegisterType<JournalEntryRouter>().AsSelf().SingleInstance();
        }
    }
}