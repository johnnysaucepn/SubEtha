using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Howatworks.Configuration;
using Howatworks.PlayerJournal.Parser;

namespace Thumb.Core
{
    public class ThumbCoreModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ThumbApp>().SingleInstance();
            builder.Register(c => new ConfigLoader("config.json")).As<IConfigLoader>().SingleInstance();
            builder.RegisterType<JournalMonitorConfiguration>().As<IJournalMonitorConfiguration>().SingleInstance();
            builder.RegisterType<JournalMonitor>().AsSelf().SingleInstance();
            builder.RegisterType<JournalParser>().As<IJournalParser>().SingleInstance();

            var plugins = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.Plugin.*.dll")
                .Select(Assembly.LoadFile);

            builder.RegisterAssemblyModules(plugins.ToArray());
        }
    }
}