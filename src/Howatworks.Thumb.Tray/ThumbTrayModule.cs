using Autofac;
using Howatworks.Thumb.Core;
using Microsoft.Extensions.Configuration;

namespace Howatworks.Thumb.Tray
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
            if (_config.GetValue<bool>("AudioNotifications"))
            {
                builder.RegisterType<WindowsThumbNotifier>().As<IThumbNotifier>().SingleInstance();
            }
            else
            {
                builder.RegisterType<SilentThumbNotifier>().As<IThumbNotifier>().SingleInstance();
            }
        }
    }
}