using System.Text.Json.Serialization;
using System.Text.Json;

namespace Net.Kigurumi.QQBot.Core.Events
{
    public class PrivateMessageEvent : BaseEvent
    {
        [JsonPropertyName("message_type")]
        public string MessageType { get; set; } = "private";

        [JsonPropertyName("sub_type")]
        public string SubType { get; set; } = "";

        [JsonPropertyName("message_id")]
        public int MessageId { get; set; }

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        [JsonPropertyName("message")]
        public JsonElement Message { get; set; }

        [JsonPropertyName("raw_message")]
        public string RawMessage { get; set; } = "";

        [JsonPropertyName("font")]
        public int Font { get; set; }
        
        [JsonIgnore]
        public string ParsedMessage
        {
            get
            {
                if (Message.ValueKind == JsonValueKind.String)
                {
                    return Message.GetString();
                }
                else if (Message.ValueKind == JsonValueKind.Array)
                {
                    var parts = new List<string>();
                    foreach (var item in Message.EnumerateArray())
                    {
                        var type = item.GetProperty("type").GetString();
                        if (type == "text")
                        {
                            parts.Add(item.GetProperty("data").GetProperty("text").GetString());
                        }
                        else
                        {
                            // 表情等
                            parts.Add($"[CQ:{type}]");
                        }
                    }
                    return string.Join("", parts);
                }
                return "";
            }
        }
    }
}
