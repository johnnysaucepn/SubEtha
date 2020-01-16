using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Howatworks.Matrix.Site.Areas.Identity.IdentityHostingStartup))]
namespace Howatworks.Matrix.Site.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}