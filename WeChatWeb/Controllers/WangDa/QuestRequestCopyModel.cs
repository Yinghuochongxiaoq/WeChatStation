using Newtonsoft.Json;

namespace WeChatWeb.Controllers.WangDa
{
    public class QuestRequestCopyModel
    {
        /// <summary>
        /// 答案id
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 选项下标
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 是否是答案
        /// </summary>
        [JsonProperty("type")]
        public string Type { get;set; }

        /// <summary>
        /// 选项
        /// </summary>
        [JsonProperty("value")]
        public string Value { get;set; }
    }
}