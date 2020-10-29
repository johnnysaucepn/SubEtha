using Autofac;
using Howatworks.Thumb.Matrix.Core;

namespace Howatworks.Thumb.Matrix.Console
{
    public class MatrixConsoleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConsoleAuthenticator>().As<IMatrixAuthenticator>().SingleInstance();
        }
    }
}
