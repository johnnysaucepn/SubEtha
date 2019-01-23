using Autofac;
using Thumb.Plugin;

namespace Thumb.Console
{
    public class ThumbConsoleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConsoleJournalMonitorNotifier>().As<IJournalMonitorNotifier>().SingleInstance();
        }
    }
}