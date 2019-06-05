using Autofac;
using Howatworks.Thumb.Plugin;

namespace Howatworks.Thumb.Tray
{
    public class ThumbTrayModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WindowsJournalMonitorNotifier>().As<IJournalMonitorNotifier>().SingleInstance();
        }
    }
}