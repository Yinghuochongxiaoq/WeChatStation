using System.Collections.Generic;
using System.Linq;
using Dapper;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;
using WeChatModel.DatabaseModel;

namespace WeChatDataAccess
{
    /// <summary>
    /// 保存信息
    /// </summary>
    public class SyscontentviewinfoData
    {
        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="model"></param>
        public void SaveViewInfoModel(SyscontentviewinfoModel model)
        {
            if (model == null) return;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                if (model.Id > 0)
                {
                    conn.Update(model);
                }
                else
                {
                    conn.Insert(model);
                }
            }
        }

        /// <summary>
        /// 获取前No个点击量的文章id
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<StatisticsViewModel> GetTopNoContentIds(int pageIndex, int pageSize)
        {
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                var sql = $"select ContentId,count(1) as ViewCount from syscontentviewinfo GROUP BY ContentId ORDER BY ViewCount desc LIMIT {(pageIndex - 1) * pageSize},{pageSize}";
                return conn.Query<StatisticsViewModel>(sql)?.ToList();
            }
        }

        /// <summary>
        /// 获取浏览数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public long? GetViewCountByContentId(long id)
        {
            if (id < 1) return 0;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                var sql = $"select ContentId,count(1) as ViewCount from syscontentviewinfo where ContentId={id} ";
                return conn.Query<StatisticsViewModel>(sql)?.FirstOrDefault()?.ViewCount;

            }
        }
    }
}
