using Autofac;
using Howatworks.Assistant.Core.ControlSimulators;
using Howatworks.Assistant.WebSockets;
using Howatworks.SubEtha.Bindings;
using log4net;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Howatworks.Assistant.Core
{
    public class AssistantModule : Module
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(AssistantModule));

        private readonly IConfiguration _config;

        public AssistantModule(IConfiguration config)
        {
            _config = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AssistantApp>().AsSelf().SingleInstance();
            builder.RegisterType<StatusManager>().AsSelf().SingleInstance();
            builder.RegisterType<ConnectionManager>().AsSelf().SingleInstance();
            builder.RegisterType<AssistantWebSocketHandler>().AsSelf().As<WebSocketHandler>().SingleInstance();
            builder.RegisterType<AssistantMessageHub>().AsSelf().SingleInstance();
            builder.RegisterType<GameControlBridge>().AsSelf().SingleInstance();
            builder.RegisterType<InputSimulatorKeyboardSimulator>().As<IVirtualKeyboardSimulator>().SingleInstance();
            builder.RegisterType<InputSimulatorMouseSimulator>().As<IVirtualMouseSimulator>().SingleInstance();
            builder.Register(_ =>
            {
                var bindingsPath = Path.Combine(_config["BindingsFolder"], _config["BindingsFilename"]);

                Log.Info($"Reading bindings from {bindingsPath}");
                return BindingMapper.FromFile(bindingsPath);
            });
            builder.RegisterType<AssistantMessageParser>().AsSelf().SingleInstance();
        }
    }
}
