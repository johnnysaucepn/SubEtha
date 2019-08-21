using Autofac;
using Howatworks.Matrix.Domain;

namespace Howatworks.Thumb.Matrix.Core
{
    public class MatrixPluginModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpUploadClient>().AsSelf().SingleInstance();

            builder.RegisterType<MatrixJournalProcessorPlugin>().AsSelf().SingleInstance();

            builder.RegisterType<CommanderTracker>().AsSelf().SingleInstance();
            builder.RegisterType<LocationManager>().AsSelf().SingleInstance();
            builder.RegisterType<ShipManager>().AsSelf().SingleInstance();
            builder.RegisterType<SessionManager>().AsSelf().SingleInstance();

            builder.RegisterType<LocationUploader>().As<IUploader<LocationState>>().SingleInstance();
            builder.RegisterType<ShipUploader>().As<IUploader<ShipState>>().SingleInstance();
            builder.RegisterType<SessionUploader>().As<IUploader<SessionState>>().SingleInstance();
        }
    }
}
