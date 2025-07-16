using System.Text.Json.Serialization;

namespace Net.Kigurumi.QQBot.Core.Events
{
    public class GroupDecreaseNoticeEvent : BaseEvent
    {
        [JsonPropertyName("notice_type")]
        public string NoticeType { get; set; } = "group_decrease";

        [JsonPropertyName("sub_type")]
        public string SubType { get; set; } = "";

        [JsonPropertyName("group_id")]
        public long GroupId { get; set; }

        [JsonPropertyName("user_id")]
        public long UserId { get; set; }

        [JsonPropertyName("operator_id")]
        public long OperatorId { get; set; }
    }
}


