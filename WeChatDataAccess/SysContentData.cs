using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.SqlHelper;
using WeChatModel.DatabaseModel;
using WeChatModel.Enum;

namespace WeChatDataAccess
{
    public class SysContentData
    {
        /// <summary>
        /// 根据id获取内容信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Syscontent GetContentModel(long id)
        {
            if (id < 1) return null;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.Get<Syscontent>(id);
            }
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="title"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="contentType"></param>
        /// <param name="contentSource"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Syscontent> GetModels(string title, string starttime, string endtime, string contentType, string contentSource, int pageIndex, int pageSize)
        {
            var where = new StringBuilder(" where IsDel=@IsDel ");
            if (!string.IsNullOrEmpty(title))
            {
                where.Append(" and Title like @Title ");
            }

            if (!string.IsNullOrEmpty(starttime))
            {
                where.Append(" and CreateTime> @StartTime ");
            }

            if (!string.IsNullOrEmpty(endtime))
            {
                where.Append(" and CreateTime< @EndTime ");
            }

            if (!string.IsNullOrEmpty(contentType))
            {
                where.Append(" and ContentType=@ContentType ");
            }

            if (!string.IsNullOrEmpty(contentSource))
            {
                where.Append(" and ContentSource=@ContentSource ");
            }
            var param = new
            {
                IsDel = FlagEnum.HadZore.GetHashCode(),
                Title = "%" + title + "%",
                ContentSource = contentSource,
                ContentType = contentType,
                StartTime = starttime,
                EndTime = endtime
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetListPaged<Syscontent>(pageIndex, pageSize, where.ToString(), " CreateTime desc ", param)?.ToList();
            }
        }

        /// <summary>
        /// 获取总记录数
        /// </summary>
        /// <returns></returns>
        public int GetCount(string title, string starttime, string endtime, string contentType, string contentSource)
        {
            var where = new StringBuilder(" where IsDel=@IsDel ");
            if (!string.IsNullOrEmpty(title))
            {
                where.Append(" and Title like @Title ");
            }

            if (!string.IsNullOrEmpty(starttime))
            {
                where.Append(" and CreateTime> @StartTime ");
            }

            if (!string.IsNullOrEmpty(endtime))
            {
                where.Append(" and CreateTime< @EndTime ");
            }

            if (!string.IsNullOrEmpty(contentType))
            {
                where.Append(" and ContentType=@ContentType ");
            }

            if (!string.IsNullOrEmpty(contentSource))
            {
                where.Append(" and ContentSource=@ContentSource ");
            }
            var param = new
            {
                IsDel = FlagEnum.HadZore.GetHashCode(),
                Title = "%" + title + "%",
                ContentSource = contentSource,
                ContentType = contentType,
                StartTime = starttime,
                EndTime = endtime
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.RecordCount<Syscontent>(where.ToString(), param);
            }
        }

        /// <summary>
        /// 获取前No个记录数
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Syscontent> GetTopNoContent(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            var viewInfoData = new SyscontentviewinfoData();
            var dataViews = viewInfoData.GetTopNoContentIds(page, pageSize);
            if (dataViews == null || dataViews.Count < 1)
            {
                return GetModels(null, null, null, null, null, page, pageSize);
            }

            var listContentIds = dataViews.Select(f => f.ContentId);
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<Syscontent>(" where Id in @Ids ", new { Ids = listContentIds })?.ToList();
            }
        }

        /// <summary>
        /// 获取前后文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Syscontent> GetPreNextContent(long id)
        {
            if (id < 1) return null;
            var result = new List<Syscontent>();
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                var pre = conn.GetListPaged<Syscontent>(1, 1, " where Id<@Id ", " CreateTime desc ", new { Id = id })?.ToList().FirstOrDefault();
                var next = conn.GetListPaged<Syscontent>(1, 1, " where Id>@Id ", " CreateTime asc ", new { Id = id })?.ToList().FirstOrDefault();
                if (pre != null) result.Add(pre);
                if (next != null) result.Add(next);
            }

            return result;
        }
    }
}
