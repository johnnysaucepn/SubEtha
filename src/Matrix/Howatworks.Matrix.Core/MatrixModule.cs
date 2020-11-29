using Autofac;
using Howatworks.Matrix.Domain;

namespace Howatworks.Matrix.Core
{
    public class MatrixModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpUploadClient>().AsSelf().SingleInstance();

            builder.RegisterType<MatrixApp>().AsSelf().SingleInstance();

            builder.RegisterType<GameContextManager>().AsSelf().SingleInstance();
            builder.RegisterType<LocationManager>().AsSelf().SingleInstance();
            builder.RegisterType<ShipManager>().AsSelf().SingleInstance();
            builder.RegisterType<SessionManager>().AsSelf().SingleInstance();
            
        }
    }
}
