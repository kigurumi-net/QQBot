using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Net.Kigurumi.QQBot.Connector.Server;

public class NapcatWsServer
{
    private readonly HttpListener _listener = new();
    private readonly string _prefix;

    public NapcatWsServer(string prefix)
    {
        _prefix = prefix;
        _listener.Prefixes.Add(prefix);
    }

    public async Task StartAsync(CancellationToken token)
    {
        _listener.Start();
        Console.WriteLine($"[Server] WS 服务监听 {_prefix}");

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

        Console.WriteLine("[Server] 客户端已连接 Napcat");

        while (socket.State == WebSocketState.Open && !token.IsCancellationRequested)
        {
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), token);
            var msg = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Console.WriteLine($"[Server 收到] {msg}");

            try
            {
                using var doc = JsonDocument.Parse(msg);
                var root = doc.RootElement;

                string postType = root.GetProperty("post_type").GetString() ?? "";

                if (root.GetProperty("post_type").GetString() == "notice" &&
                    root.GetProperty("notice_type").GetString() == "group_increase" &&
                    root.GetProperty("sub_type").GetString() == "invite")
                {
                    var groupId = root.GetProperty("group_id").GetInt64();

                    var reply = new
                    {
                        action = "send_group_msg",
                        @params = new
                        {
                            group_id = groupId,
                            message = "大家好，我是机器人，感谢邀请！"
                        },
                        echo = "group-welcome"
                    };

                    string json = JsonSerializer.Serialize(reply);
                    await socket.SendAsync(
                        Encoding.UTF8.GetBytes(json),
                        WebSocketMessageType.Text,
                        true,
                        token);
                }

                if (postType == "message")
                {
                    await OnMessageAsync(socket, root, token);
                }
                else
                {
                    Console.WriteLine($"[OneBot] 忽略非 message 类型事件：{postType}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[错误] 消息处理失败：{ex.Message}");
            }
        }

        Console.WriteLine("[Server] 客户端连接断开");
    }

    /// <summary>
    /// 处理 OneBot 消息事件
    /// </summary>
    protected virtual async Task OnMessageAsync(WebSocket socket, JsonElement message, CancellationToken token)
    {
        var userId = message.GetProperty("user_id").GetInt64();
        var rawMessage = message.GetProperty("raw_message").GetString() ?? "";

        Console.WriteLine($"[私聊消息] 来自 {userId}：{rawMessage}");

        // 示例：构造回复消息（你可以改为调用后端接口）
        var response = new
        {
            action = "send_private_msg",
            @params = new
            {
                user_id = userId,
                message = $"你说的是：{rawMessage}"
            },
            echo = "reply-echo"
        };

        var json = JsonSerializer.Serialize(response);
        var jsonBytes = Encoding.UTF8.GetBytes(json);

        await socket.SendAsync(new ArraySegment<byte>(jsonBytes), WebSocketMessageType.Text, true, token);
    }
}
