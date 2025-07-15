using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Net.Kigurumi.QQBot.Core.Events
{
    public class GroupRequestEvent : BaseEvent
    {
        [JsonPropertyName("request_type")]
        public string RequestType { get; set; } = "group";

        [JsonPropertyName("sub_type")]
        public string SubType { get; set; } = "";

        [JsonPropertyName("group_id")]
        public long GroupId { get; set; }

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; } = "";

        [JsonPropertyName("flag")]
        public string Flag { get; set; } = "";
    }
}
