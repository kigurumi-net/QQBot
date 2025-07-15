using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Net.Kigurumi.QQBot.Connector.Client
{
    public static class NapcatWsClient
    {
        public static async Task RunAsync(string uri, CancellationToken token)
        {
            using var ws = new ClientWebSocket();
            await ws.ConnectAsync(new Uri(uri), token);

            var buffer = new byte[4096];
            while (!token.IsCancellationRequested)
            {
                var result = await ws.ReceiveAsync(buffer, token);
                var msg = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"[Client 收到] {msg}");

                // 这里可调用 Core 逻辑处理
            }
        }
    }
}
