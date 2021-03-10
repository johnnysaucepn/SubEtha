using log4net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Howatworks.Thumb.Core
{
    public static class ThumbLoggingExtensions
    {
        public static ILoggingBuilder UseThumbLogging(this ILoggingBuilder builder, IConfiguration config)
        {
            var logFolder = config["LogFolder"];
            Directory.CreateDirectory(logFolder);

            GlobalContext.Properties["logfolder"] = logFolder;
            return builder;
        }
    }
}
