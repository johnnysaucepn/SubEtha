using Autofac;
using Howatworks.Thumb.Plugin;

namespace Howatworks.Thumb.Console
{
    public class ThumbConsoleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConsoleJournalMonitorNotifier>().As<IJournalMonitorNotifier>().SingleInstance();
        }
    }
}