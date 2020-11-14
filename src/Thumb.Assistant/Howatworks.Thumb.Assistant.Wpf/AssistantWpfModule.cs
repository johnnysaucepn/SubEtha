using Autofac;
using Howatworks.Thumb.Assistant.Core;

namespace Howatworks.Thumb.Assistant.Wpf
{
    public class AssistantWpfModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => TrayIconViewModel.Create(c.Resolve<AssistantApp>()));
        }
    }
}
