using Autofac;
using Howatworks.Matrix.Domain;

namespace Howatworks.Thumb.Matrix.Core
{
    public class MatrixModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpUploadClient>().AsSelf().SingleInstance();

            builder.RegisterType<MatrixApp>().AsSelf().SingleInstance();

            builder.RegisterType<CommanderTracker>().AsSelf().SingleInstance();
            builder.RegisterType<LocationManager>().AsSelf().SingleInstance();
            builder.RegisterType<ShipManager>().AsSelf().SingleInstance();
            builder.RegisterType<SessionManager>().AsSelf().SingleInstance();

            builder.RegisterType<LocationUploadQueue>().As<UploadQueue<LocationState>>().SingleInstance();
            builder.RegisterType<ShipUploadQueue>().As<UploadQueue<ShipState>>().SingleInstance();
            builder.RegisterType<SessionUploadQueue>().As<UploadQueue<SessionState>>().SingleInstance();
        }
    }
}
