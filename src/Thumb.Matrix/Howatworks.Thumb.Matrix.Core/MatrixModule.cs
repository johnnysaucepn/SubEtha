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

            builder.RegisterType<GameContextTracker>().AsSelf().SingleInstance();
            builder.RegisterType<LocationManager>().AsSelf().SingleInstance();
            builder.RegisterType<ShipManager>().AsSelf().SingleInstance();
            builder.RegisterType<SessionManager>().AsSelf().SingleInstance();
            
            builder.RegisterType<Tracker<LocationState>>().AsSelf().SingleInstance();
            builder.RegisterType<Tracker<ShipState>>().AsSelf().SingleInstance();
            builder.RegisterType<Tracker<SessionState>>().AsSelf().SingleInstance();
        }
    }
}
