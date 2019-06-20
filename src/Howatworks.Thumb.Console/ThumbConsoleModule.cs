using Autofac;
using Howatworks.Thumb.Core;

namespace Howatworks.Thumb.Console
{
    public class ThumbConsoleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConsoleThumbNotifier>().As<IThumbNotifier>().SingleInstance();
        }
    }
}