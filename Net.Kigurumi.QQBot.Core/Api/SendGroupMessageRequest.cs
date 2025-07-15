using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Net.Kigurumi.QQBot.Core.Api
{
    public class SendGroupMessageRequest
    {
        [JsonPropertyName("action")]
        public string Action => "send_group_msg";

        [JsonPropertyName("params")]
        public ParamsData Params { get; set; } = new();

        public class ParamsData
        {
            [JsonPropertyName("group_id")]
            public long GroupId { get; set; }

            [JsonPropertyName("message")]
            public string Message { get; set; } = "";
        }
    }
}
