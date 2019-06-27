/*using Autofac;
using Microsoft.Extensions.Configuration;
using Module = Autofac.Module;

namespace Howatworks.Matrix.Site
{
    public class ServiceModule : Module
    {
        private readonly IConfiguration _config;

        public ServiceModule(IConfiguration config)
        {
            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => _config).As<IConfiguration>().SingleInstance();
        }

    }
}*/
