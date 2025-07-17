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

            if (root.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in root.EnumerateArray())
                {
                    await DispatchSingleAsync(item);
                }
            }
            else if (root.ValueKind == JsonValueKind.Object)
            {
                await DispatchSingleAsync(root);
            }
        }

        private async Task DispatchSingleAsync(JsonElement root)
        {
            if (!root.TryGetProperty("post_type", out var postTypeProp))
                return;

            var postType = postTypeProp.GetString();
            var json = root.GetRawText(); // 重新序列化当前事件

            switch (postType)
            {
                case "message_sent":
                    var sentType = root.GetProperty("message_sent_type").GetString(); // 可选使用
                    var sentMessageType = root.GetProperty("message_type").GetString();
                    if (sentMessageType == "group")
                    {
                        var sentMsg = JsonSerializer.Deserialize<GroupMessageEvent>(json);
                        if (sentMsg != null)
                            await _handler.OnGroupMessageSentAsync(sentMsg);
                    }
                    else if (sentMessageType == "private")
                    {
                        var sentMsg = JsonSerializer.Deserialize<PrivateMessageEvent>(json);
                        if (sentMsg != null)
                            await _handler.OnPrivateMessageSentAsync(sentMsg);
                    }
                    break;
                
                case "message":
                    var messageType = root.GetProperty("message_type").GetString();
                    if (messageType == "group")
                    {
                        var groupMsg = JsonSerializer.Deserialize<GroupMessageEvent>(json);
                        if (groupMsg != null)
                            await _handler.OnGroupMessageAsync(groupMsg);
                    }
                    else if (messageType == "private")
                    {
                        var privateMsg = JsonSerializer.Deserialize<PrivateMessageEvent>(json);
                        if (privateMsg != null)
                            await _handler.OnPrivateMessageAsync(privateMsg);
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

                case "notice":
                    var noticeType = root.GetProperty("notice_type").GetString();
                    switch (noticeType)
                    {
                        case "group_increase":
                            var increase = JsonSerializer.Deserialize<GroupIncreaseNoticeEvent>(json);
                            if (increase != null)
                                await _handler.OnGroupIncreaseAsync(increase);
                            break;

                        case "group_decrease":
                            var decrease = JsonSerializer.Deserialize<GroupDecreaseNoticeEvent>(json);
                            if (decrease != null)
                                await _handler.OnGroupDecreaseAsync(decrease);
                            break;
                    }
                    break;

                case "meta_event":
                    var meta = JsonSerializer.Deserialize<MetaEvent>(json);
                    if (meta != null)
                        await _handler.OnMetaEventAsync(meta);
                    break;
            }
        }
    }
}
