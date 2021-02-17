using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Howatworks.Assistant.Core
{
    public class AssistantBackgroundService : BackgroundService
    {
        private readonly AssistantApp _app;

        public AssistantBackgroundService(AssistantApp app)
        {
            _app = app;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _app.RunAsync(stoppingToken).ConfigureAwait(false);
        }
    }
}
