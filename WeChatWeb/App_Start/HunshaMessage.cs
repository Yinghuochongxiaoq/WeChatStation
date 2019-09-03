using System;
using FreshCommonUtility.Dapper;

namespace WeChatWeb
{
    /// <summary>
    /// SysUser table in MySQL5
    /// </summary>
    public class HunshaMessage
    {
        /// <summary>
        /// Id 主键，用户id
        /// </summary>
        [Key]
        public Int32 Id { get; set; }
        /// <summary>
        /// UserName 用户名
        /// </summary>
        public String Message { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        public String Prince { get; set; }
    }
}