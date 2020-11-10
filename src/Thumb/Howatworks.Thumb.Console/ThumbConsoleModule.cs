using Autofac;
using Microsoft.Extensions.Configuration;

namespace Howatworks.Thumb.Console
{
    public class ThumbConsoleModule : Module
    {
        private readonly IConfiguration _config;

        public ThumbConsoleModule(IConfiguration config)
        {
            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConsoleKeyListener>().AsSelf().SingleInstance();
        }
    }
}