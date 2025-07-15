using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Net.Kigurumi.QQBot.Core.Api
{
    public class SetGroupAddRequest
    {
        [JsonPropertyName("action")]
        public string Action => "set_group_add_request";

        [JsonPropertyName("params")]
        public ParamsData Params { get; set; } = new();

        public class ParamsData
        {
            [JsonPropertyName("flag")]
            public string Flag { get; set; } = "";

            [JsonPropertyName("sub_type")]
            public string SubType { get; set; } = "add";

            [JsonPropertyName("approve")]
            public bool Approve { get; set; }

            [JsonPropertyName("reason")]
            public string? Reason { get; set; }
        }
    }
}
