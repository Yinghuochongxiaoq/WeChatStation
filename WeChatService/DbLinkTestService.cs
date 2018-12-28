using System;
using Dapper;
using FreshCommonUtility.Configure;
using FreshCommonUtility.SqlHelper;

namespace WeChatService
{
    /// <summary>
    /// 数据库连接测试
    /// </summary>
    public class DbLinkTestService
    {
        /// <summary>
        /// 数据库连接测试
        /// </summary>
        public static void DbLink()
        {
            var linkStr = AppConfigurationHelper.GetString("DbString");
            var dual = AppConfigurationHelper.GetString("DbDual");
            using (var conn = SqlConnectionHelper.GetOpenConnection(linkStr))
            {
                var data = conn.Query(dual);
                Console.WriteLine(data);
            }
        }
    }
}
