using Autofac;
using Howatworks.Thumb.Core;
using Howatworks.Thumb.Plugin;
using Howatworks.Thumb.Tray;
using Microsoft.Extensions.Configuration;

namespace Howatworks.Thumb.Forms
{
    public class ThumbFormsModule : Module
    {
        private readonly IConfiguration _config;

        public ThumbFormsModule(IConfiguration config)
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