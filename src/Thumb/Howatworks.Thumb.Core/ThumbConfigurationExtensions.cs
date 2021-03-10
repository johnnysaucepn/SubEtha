using Microsoft.Extensions.Configuration;

namespace Howatworks.Thumb.Core
{
    public static class ThumbConfigurationExtensions
    {
        public static IConfigurationBuilder AddThumbConfiguration(this IConfigurationBuilder builder, string appName)
        {
            var context = new ThumbConfigurationContext(appName);

            return builder
                .AddInMemoryCollection(context.GetDefaultConfig())
                .AddEnvironmentVariables("THUMB_");
        }
    }
}
