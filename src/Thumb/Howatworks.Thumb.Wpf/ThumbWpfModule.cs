using Autofac;
using Howatworks.Thumb.Core;
using Microsoft.Extensions.Configuration;

namespace Howatworks.Thumb.Wpf
{
    public class ThumbWpfModule : Module
    {
        private readonly IConfiguration _config;

        public ThumbWpfModule(IConfiguration config)
        {
            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (_config.GetValue<bool>("AudioNotifications"))
            {
                builder.RegisterType<WindowsThumbNotifier>().As<IThumbNotifier>().SingleInstance();
            }
        }
    }
}