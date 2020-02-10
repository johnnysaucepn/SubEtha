using Autofac;
using Howatworks.Thumb.Core;

namespace Howatworks.Thumb.Matrix.Win
{
    public class MatrixWpfModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SilentThumbNotifier>().As<IThumbNotifier>().SingleInstance();
        }
    }
}
