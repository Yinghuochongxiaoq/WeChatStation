using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;
using WeChatModel.DatabaseModel;
using WeChatModel.Enum;

namespace WeChatDataAccess
{
    /// <summary>
    /// 广告数据服务
    /// </summary>
    public class SysAdvertiseData
    {
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<SysadvertisementModel> GetModels(string type, int pageIndex, int pageSize)
        {
            var where = new StringBuilder(" where IsDel=@IsDel ");

            if (!string.IsNullOrEmpty(type))
            {
                where.Append(" and AdvertiType= @Type ");
            }
            var param = new
            {
                IsDel = FlagEnum.HadZore.GetHashCode(),
                Type = type
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetListPaged<SysadvertisementModel>(pageIndex, pageSize, where.ToString(), " Sort desc ", param)?.ToList();
            }
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <returns></returns>
        public int GetCount(string type)
        {
            var where = new StringBuilder(" where IsDel=@IsDel ");

            if (!string.IsNullOrEmpty(type))
            {
                where.Append(" and AdvertiType= @Type ");
            }
            var param = new
            {
                IsDel = FlagEnum.HadZore.GetHashCode(),
                Type = type
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.RecordCount<SysadvertisementModel>(where.ToString(), param);
            }
        }
    }
}
