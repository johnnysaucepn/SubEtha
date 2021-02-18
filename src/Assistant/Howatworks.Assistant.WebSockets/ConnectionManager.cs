using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Howatworks.Assistant.WebSockets
{
    /// <summary>
    /// Adapted from code published at https://radu-matei.com/blog/aspnet-core-websockets-middleware
    /// </summary>
    public class ConnectionManager
    {
        private readonly ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        public WebSocket GetSocketById(string id)
        {
            return _sockets.FirstOrDefault(p => p.Key == id).Value;
        }

        public ConcurrentDictionary<string, WebSocket> GetAll()
        {
            return _sockets;
        }

        public string GetId(WebSocket socket)
        {
            return _sockets.FirstOrDefault(p => p.Value == socket).Key;
        }
        public async Task AddSocket(WebSocket socket)
        {
            await Task.Run(() => _sockets.TryAdd(CreateConnectionId(), socket)).ConfigureAwait(false);
        }

        public async Task RemoveSocket(string id)
        {
            if (_sockets.TryRemove(id, out var socket))
            {
                await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                        statusDescription: "Closed by the ConnectionManager",
                                        cancellationToken: CancellationToken.None).ConfigureAwait(false);
            }
        }

        public async Task RemoveAllSockets()
        {
            while (_sockets.Count > 0)
            {
                var id = _sockets.Keys.FirstOrDefault();
                if (!string.IsNullOrEmpty(id))
                {
                    await RemoveSocket(id).ConfigureAwait(false);
                }
            }
        }

        private string CreateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
