using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Howatworks.Matrix.Core
{
    public class MatrixBackgroundService : BackgroundService
    {
        private readonly MatrixApp _app;

        public MatrixBackgroundService(MatrixApp app)
        {
            _app = app;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() => _app.Run(stoppingToken)).ConfigureAwait(false);
        }
    }
}
