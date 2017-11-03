using System.IO;
using System.Reflection;
using Autofac;
using Howatworks.Configuration;
using Module = Autofac.Module;

namespace SubEtha.Service
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var appFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            builder.Register(c => new ConfigLoader(Path.Combine(appFolder,"config.json")))
                .As<IConfigLoader>().SingleInstance();
        }

    }
}
