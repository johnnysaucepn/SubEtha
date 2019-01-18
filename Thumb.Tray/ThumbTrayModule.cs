using Autofac;
using Microsoft.Extensions.Configuration;
using Thumb.Core;
using Thumb.Plugin;

namespace Thumb.Tray
{
    public class ThumbTrayModule : Module
    {
        private readonly IConfiguration _config;

        public ThumbTrayModule(IConfiguration config)
        {
            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WindowsJournalMonitorNotifier>().As<IJournalMonitorNotifier>().SingleInstance();
            builder.Register(c => _config).As<IConfiguration>().SingleInstance();
        }
    }
}