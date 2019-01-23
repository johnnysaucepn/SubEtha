using Autofac;
using Microsoft.Extensions.Configuration;

namespace SubEtha.Site
{
    public class SiteAutofacModule : Module
    {
        private readonly IConfiguration _config;

        public SiteAutofacModule(IConfiguration config)
        {
            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => _config).As<IConfiguration>().SingleInstance();
        }
    }
}
