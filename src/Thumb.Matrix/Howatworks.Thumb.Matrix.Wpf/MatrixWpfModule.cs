using Autofac;
using Howatworks.Thumb.Core;

namespace Howatworks.Thumb.Matrix.Wpf
{
    public class MatrixWpfModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TrayIconViewModel>().AsSelf().SingleInstance();

            builder.RegisterType<AuthenticationDialog>().AsSelf().SingleInstance();
            builder.RegisterType<AuthenticationDialogViewModel>().AsSelf().SingleInstance();
        }
    }
}
