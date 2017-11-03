using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Howatworks.Configuration;

namespace SubEtha.Site
{
    public class SiteAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new ConfigLoader("config.json")).As<IConfigLoader>().SingleInstance();
        }
    }
}
