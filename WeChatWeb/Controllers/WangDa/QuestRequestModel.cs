using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace WeChatWeb.Controllers.WangDa
{
    /// <summary>
    /// 题目请求信息
    /// </summary>
    public class QuestRequestModel
    {
        /// <summary>
        /// 题目id
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 题目内容
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>
        /// 题目答案
        /// </summary>
        [JsonProperty("questionAttrCopys")]
        public List<QuestRequestCopyModel> QuestionAttrCopys { get; set; }
    }
}