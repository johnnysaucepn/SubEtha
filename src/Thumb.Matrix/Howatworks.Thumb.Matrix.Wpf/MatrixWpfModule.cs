using Autofac;
using Howatworks.Thumb.Matrix.Core;

namespace Howatworks.Thumb.Matrix.Wpf
{
    public class MatrixWpfModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => TrayIconViewModel.Create(c.Resolve<MatrixApp>()));

            builder.RegisterType<AuthenticationDialog>().AsSelf().SingleInstance();
            builder.RegisterType<AuthenticationDialogViewModel>().AsSelf().SingleInstance();

            builder.RegisterType<DialogAuthenticator>().As<IMatrixAuthenticator>().SingleInstance();
        }
    }
}
