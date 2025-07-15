using Net.Kigurumi.QQBot.Core.Events;
using Net.Kigurumi.QQBot.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Net.Kigurumi.QQBot.Core
{
    public class OneBotEventDispatcher
    {
        private readonly IOneBotEventHandler _handler;

        public OneBotEventDispatcher(IOneBotEventHandler handler)
        {
            _handler = handler;
        }

        public async Task DispatchAsync(string json)
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("post_type", out var postTypeProp))
                return;

            var postType = postTypeProp.GetString();

            switch (postType)
            {
                case "message":
                    var messageType = root.GetProperty("message_type").GetString();
                    if (messageType == "group")
                    {
                        var groupMsg = JsonSerializer.Deserialize<GroupMessageEvent>(json);
                        if (groupMsg != null)
                            await _handler.OnGroupMessageAsync(groupMsg);
                    }
                    break;

                case "request":
                    var requestType = root.GetProperty("request_type").GetString();
                    if (requestType == "group")
                    {
                        var req = JsonSerializer.Deserialize<GroupRequestEvent>(json);
                        if (req != null)
                            await _handler.OnGroupRequestAsync(req);
                    }
                    break;

                default:
                    // 其他 post_type（如 notice、meta_event）暂不处理
                    break;
            }
        }
    }
}
