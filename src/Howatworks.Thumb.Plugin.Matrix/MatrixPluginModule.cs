using Autofac;

namespace Howatworks.Thumb.Plugin.Matrix
{
    public class MatrixPluginModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpUploadClient>().AsSelf().SingleInstance();

            builder.RegisterType<MatrixJournalProcessorPlugin>().As<IJournalProcessorPlugin>().SingleInstance();

            builder.RegisterType<LocationManager>().AsSelf().SingleInstance();
            builder.RegisterType<ShipManager>().AsSelf().SingleInstance();
            builder.RegisterType<SessionManager>().AsSelf().SingleInstance();

            builder.RegisterType<LocationHttpUploader>().As<IUploader<LocationState>>().SingleInstance();
            builder.RegisterType<ShipHttpUploader>().As<IUploader<ShipState>>().SingleInstance();
            builder.RegisterType<SessionHttpUploader>().As<IUploader<SessionState>>().SingleInstance();
        }
    }
}
