using Autofac;
using Thumb.Plugin;

namespace Thumb.Tray
{
    public class ThumbTrayModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WindowsJournalMonitorNotifier>().As<IJournalMonitorNotifier>().SingleInstance();
        }
    }
}