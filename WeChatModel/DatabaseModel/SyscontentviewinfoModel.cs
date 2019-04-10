/*|========================================================|
  |=============This code is auto by CodeBuilder===========|
  |================ Organization:FreshManIT+  =============|
  |==========Any Question please tell me:FreshManIT========|
  |===https://github.com/FreshManIT/CodeBuilder/issues ====|
  |===============OR Email:qinbocai@sina.cn================|
  |========================================================|
**/
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeChatModel.DatabaseModel
{
    /// <summary>
    /// syscontentviewinfo table in MySQL5
    /// </summary>
    [Table("syscontentviewinfo")]
    public class SyscontentviewinfoModel
    {
        /// <summary>
        /// Id 创建时间
        /// </summary>
        public Int64 Id { get; set; }
        /// <summary>
        /// ContentId 文章id
        /// </summary>
        public Int64 ContentId { get; set; }
        /// <summary>
        /// Ip 浏览ip
        /// </summary>
        public String Ip { get; set; }
        /// <summary>
        /// CreateTime 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// Browser 浏览器类型
        /// </summary>
        public String Browser { get; set; }
    }
}