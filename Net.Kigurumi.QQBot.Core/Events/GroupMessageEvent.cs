using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Net.Kigurumi.QQBot.Core.Events
{
    public class GroupMessageEvent : BaseEvent
    {
        [JsonPropertyName("message_type")]
        public string MessageType { get; set; } = "group";

        [JsonPropertyName("sub_type")]
        public string SubType { get; set; } = "";

        [JsonPropertyName("message_id")]
        public int MessageId { get; set; }

        [JsonPropertyName("group_id")]
        public long GroupId { get; set; }

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = "";

        [JsonPropertyName("raw_message")]
        public string RawMessage { get; set; } = "";

        [JsonPropertyName("font")]
        public int Font { get; set; }
    }
}
