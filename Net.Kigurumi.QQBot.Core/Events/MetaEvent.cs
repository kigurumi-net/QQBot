using System.Text.Json.Serialization;

namespace Net.Kigurumi.QQBot.Core.Events
{
    public class MetaEvent : BaseEvent
    {
        [JsonPropertyName("meta_event_type")]
        public string MetaEventType { get; set; } = "";

        [JsonPropertyName("interval")]
        public int? Interval { get; set; } // 心跳间隔（仅当 meta_event_type 为 heartbeat 时出现）
    }
}