using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Howatworks.Configuration;
using Howatworks.PlayerJournal.Monitor;
using Howatworks.PlayerJournal.Parser;
using Microsoft.Extensions.Configuration;
using Thumb.Plugin;

namespace Thumb.Core
{
    public class ThumbCoreModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ThumbApp>().SingleInstance();
            builder.Register(c => new ConfigLoader("config.json")).As<IConfigLoader>().SingleInstance();
            builder.RegisterType<JsonJournalMonitorState>().As<IJournalMonitorState>().SingleInstance();
            builder.RegisterType<JournalMonitorScheduler>().AsSelf().SingleInstance();
            builder.RegisterType<JournalParser>().As<IJournalParser>().SingleInstance();
            builder.RegisterType<JournalReaderFactory>().As<IJournalReaderFactory>().SingleInstance();
            builder.RegisterType<ThumbProcessor>().AsSelf().SingleInstance();

            var plugins = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.Plugin.*.dll")
                .Select(Assembly.LoadFile);

            builder.RegisterAssemblyModules(plugins.ToArray());
        }
    }
}