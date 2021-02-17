using Howatworks.Assistant.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Howatworks.Assistant.Console
{
    internal class ConsoleWorker : BackgroundService
    {
        private readonly AssistantApp _app;

        public ConsoleWorker(AssistantApp app)
        {
            _app = app;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _app.RunAsync(stoppingToken).ConfigureAwait(false);
        }
    }
}
