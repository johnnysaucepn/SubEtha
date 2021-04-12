using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Howatworks.Assistant.Core
{
    public class AssistantBackgroundService : BackgroundService
    {
        private readonly AssistantApp _app;
        private readonly ILogger<AssistantBackgroundService> _logger;

        public AssistantBackgroundService(AssistantApp app, ILogger<AssistantBackgroundService> logger)
        {
            _app = app;
            _logger = logger;
        }

        /// <summary>
        /// Execute the background service.
        /// </summary>
        /// <remarks>
        /// ExecuteAsync must not block, as this will prevent the host from completing startup.
        /// This code uses Task.Run to ensure that the execution runs in the background.
        /// See: https://blog.stephencleary.com/2020/05/backgroundservice-gotcha-startup.html for details.
        /// </remarks>
        /// <param name="stoppingToken"></param>
        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(async () =>
        {
            try
            {
                await _app.RunAsync(stoppingToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation($"{nameof(AssistantBackgroundService)} cancelled");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Fatal error");
            }
        });
    }
}
