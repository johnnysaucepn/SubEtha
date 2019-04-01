using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Howatworks.PlayerJournal.Monitor;
using Howatworks.PlayerJournal.Parser;
using Microsoft.Extensions.Configuration;

namespace Thumb.Core
{
    public class ThumbCoreModule : Autofac.Module
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
            builder.RegisterType<ThumbProcessor>().AsSelf().SingleInstance();
            builder.RegisterType<JournalEntryRouter>().AsSelf().SingleInstance();

            var plugins = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.Plugin.*.dll")
                .Select(Assembly.LoadFile);

            builder.RegisterAssemblyModules(plugins.ToArray());
        }
    }
}