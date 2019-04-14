using Autofac;

namespace Howatworks.Thumb.Plugin.Matrix
{
    public class MatrixPluginModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpUploadClient>().AsSelf().SingleInstance();

            builder.RegisterType<MatrixJournalProcessorPlugin>().As<IJournalProcessorPlugin>().SingleInstance();
        }
    }
}
