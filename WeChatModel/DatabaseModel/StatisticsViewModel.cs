namespace WeChatModel.DatabaseModel
{
    /// <summary>
    /// 统计实体
    /// </summary>
    public class StatisticsViewModel
    {
        /// <summary>
        /// 文章id
        /// </summary>
        public long ContentId { get; set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        public long ViewCount { get; set; }
    }
}
