using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Howatworks.Thumb.Assistant
{
    public class AssistantFormsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AssistantApplicationContext>().AsSelf().SingleInstance();
        }
    }
}
