using Net.Kigurumi.QQBot.Core;
using Net.Kigurumi.QQBot.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace Net.Kigurumi.QQBot.Connector.Server
{
    /// <summary>
    /// 用于通过当前 WebSocket 向 Napcat 主动发送 OneBot API 请求
    /// </summary>
    public class NapcatApiSender : IOneBotApiSender
    {
        private readonly Func<WebSocket?> _getSocket;

        public NapcatApiSender(Func<WebSocket?> getSocket)
        {
            _getSocket = getSocket;
        }

        public async Task SendAsync<T>(T apiRequest)
        {
            var socket = _getSocket();
            if (socket == null || socket.State != WebSocketState.Open)
                return;

            var json = OneBotSerializer.Serialize(apiRequest);
            var bytes = Encoding.UTF8.GetBytes(json);

            await socket.SendAsync(
                new ArraySegment<byte>(bytes),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
        }
    }
}
