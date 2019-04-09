using System;
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
    /// 数据字典
    /// </summary>
    public class SysDicData
    {
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="lable"></param>
        /// <param name="type"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<SysdictModel> GetModels(string lable, string type, int pageIndex, int pageSize)
        {
            var where = new StringBuilder(" where IsDel=@IsDel ");
            if (!string.IsNullOrEmpty(lable))
            {
                where.Append(" and Lable like @Lable ");
            }

            if (!string.IsNullOrEmpty(type))
            {
                where.Append(" and Type= @Type ");
            }
            var param = new
            {
                IsDel = FlagEnum.HadZore.GetHashCode(),
                Lable = "%" + lable + "%",
                Type = type
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetListPaged<SysdictModel>(pageIndex, pageSize, where.ToString(), null, param)?.ToList();
            }
        }

        /// <summary>
        /// 获取字典记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysdictModel Get(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.Get<SysdictModel>(id);
            }
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <returns></returns>
        public int GetCount(string lable, string type)
        {
            var where = new StringBuilder(" where IsDel=@IsDel ");
            if (!string.IsNullOrEmpty(lable))
            {
                where.Append(" and Lable like @Lable ");
            }

            if (!string.IsNullOrEmpty(type))
            {
                where.Append(" and Type= @Type ");
            }
            var param = new
            {
                IsDel = FlagEnum.HadZore.GetHashCode(),
                Lable = "%" + lable + "%",
                Type = type
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.RecordCount<SysdictModel>(where.ToString(), param);
            }
        }

        /// <summary>
        /// 获取所有的字典数据
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public List<SysdictModel> GetAllDicType(string type)
        {
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return string.IsNullOrEmpty(type)
                    ? conn.GetList<SysdictModel>(new { IsDel = FlagEnum.HadZore.GetHashCode() })?.ToList()
                    : conn.GetList<SysdictModel>(new { IsDel = FlagEnum.HadZore.GetHashCode(), Type = type })?.ToList();
            }
        }

        /// <summary>
        /// 根据value查询字典数据
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public List<SysdictModel> GetDicByValue(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<SysdictModel>(new { IsDel = FlagEnum.HadZore.GetHashCode(), Value = value })?.ToList();
            }
        }
    }
}
