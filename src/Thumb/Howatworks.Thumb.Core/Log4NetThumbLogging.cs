using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace Howatworks.Thumb.Core
{

    public class Log4NetThumbLogging : IThumbLogging
    {
        private readonly IConfiguration _config;

        public Log4NetThumbLogging(IConfiguration config)
        {
            _config = config;
        }

        public void Configure()
        {
            var logFolder = _config["LogFolder"];
            Directory.CreateDirectory(logFolder);

            GlobalContext.Properties["logfolder"] = logFolder;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
    }
}
