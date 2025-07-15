using System.Net;
using System.Net.WebSockets;
using System.Text;
using Net.Kigurumi.QQBot.Core;
using Net.Kigurumi.QQBot.Core.Interfaces;

namespace Net.Kigurumi.QQBot.Connector.Server;

public class NapcatWsServer
{
    private readonly HttpListener _listener = new();
    private readonly string _prefix;
    private readonly OneBotEventDispatcher _dispatcher;

    private WebSocket? _currentSocket;

    public NapcatWsServer(string prefix, IOneBotEventHandler handler)
    {
        _prefix = prefix;
        _listener.Prefixes.Add(prefix);
        _dispatcher = new OneBotEventDispatcher(handler);
    }

    /// <summary>
    /// 启动 WS 服务监听
    /// </summary>
    public async Task StartAsync(CancellationToken token)
    {
        _listener.Start();
        Console.WriteLine($"[Server] WS 服务监听 {_prefix}");

        while (!token.IsCancellationRequested)
        {
            var ctx = await _listener.GetContextAsync();
            if (ctx.Request.IsWebSocketRequest)
                _ = HandleClientAsync(ctx, token);
            else
            {
                ctx.Response.StatusCode = 400;
                ctx.Response.Close();
            }
        }
    }

    /// <summary>
    /// 当前连接的 API 发送器（可供外部调用）
    /// </summary>
    public IOneBotApiSender ApiSender => new NapcatApiSender(() => _currentSocket);

    /// <summary>
    /// 处理客户端连接（Napcat）
    /// </summary>
    private async Task HandleClientAsync(HttpListenerContext ctx, CancellationToken token)
    {
        var wsContext = await ctx.AcceptWebSocketAsync(null);
        _currentSocket = wsContext.WebSocket;

        var buffer = new byte[4096];
        Console.WriteLine("[Server] Napcat 已连接到当前程序");

        while (_currentSocket.State == WebSocketState.Open && !token.IsCancellationRequested)
        {
            try
            {
                var result = await _currentSocket.ReceiveAsync(new ArraySegment<byte>(buffer), token);
                var msg = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"[OneBot 收到] {msg}");

                await _dispatcher.DispatchAsync(msg); // 分发事件
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[错误] 消息处理失败：{ex.Message}");
                break;
            }
        }

        Console.WriteLine("[Server] Napcat 连接断开，将停止服务端");
        _currentSocket?.Dispose();
        _currentSocket = null;
    }
}
