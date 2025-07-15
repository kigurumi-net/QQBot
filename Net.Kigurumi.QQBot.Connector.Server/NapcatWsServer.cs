using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Net.Kigurumi.QQBot.Connector.Server
{
    public class NapcatWsServer
    {
        private readonly HttpListener _listener = new();
        private readonly int _port;

        public NapcatWsServer(int port)
        {
            _port = port;
            _listener.Prefixes.Add($"http://+:{port}/ws/");
        }

        public async Task StartAsync(CancellationToken token)
        {
            _listener.Start();
            Console.WriteLine($"[Server] WS 服务监听 ws://localhost:{_port}/ws");

            while (!token.IsCancellationRequested)
            {
                var ctx = await _listener.GetContextAsync();
                if (ctx.Request.IsWebSocketRequest)
                {
                    _ = HandleClientAsync(ctx, token);
                }
                else
                {
                    ctx.Response.StatusCode = 400;
                    ctx.Response.Close();
                }
            }
        }

        private async Task HandleClientAsync(HttpListenerContext ctx, CancellationToken token)
        {
            var wsContext = await ctx.AcceptWebSocketAsync(null);
            var socket = wsContext.WebSocket;

            var buffer = new byte[4096];
            while (socket.State == WebSocketState.Open && !token.IsCancellationRequested)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), token);
                var msg = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"[Server 收到] {msg}");

                var reply = Encoding.UTF8.GetBytes("{\"status\":\"ok\"}");
                await socket.SendAsync(reply, WebSocketMessageType.Text, true, token);
            }
        }
    }
}
