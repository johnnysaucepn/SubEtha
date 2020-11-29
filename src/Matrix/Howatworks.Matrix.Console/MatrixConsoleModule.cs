using Autofac;
using Howatworks.Matrix.Core;

namespace Howatworks.Matrix.Console
{
    public class MatrixConsoleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConsoleAuthenticator>().As<IMatrixAuthenticator>().SingleInstance();
        }
    }
}
