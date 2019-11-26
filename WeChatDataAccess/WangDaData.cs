using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using FreshCommonUtility.Dapper;
using FreshCommonUtility.DataConvert;
using FreshCommonUtility.SqlHelper;
using WeChatModel.DatabaseModel;

namespace WeChatDataAccess
{
    /// <summary>
    /// 网大题目数据服务
    /// </summary>
    public class WangDaData
    {
        /// <summary>
        /// 根据id获取内容信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WangdaquestionModel GetWangDaQuestionModel(long id)
        {
            if (id < 1) return null;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.Get<WangdaquestionModel>(id);
            }
        }

        /// <summary>
        /// 根据题目id获取内容信息
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public WangdaquestionModel GetWangDaQuestionModelByQuestionId(string questionId)
        {
            if (string.IsNullOrEmpty(questionId)) return null;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                var where = new StringBuilder(" where questionId=@questionId ");
                var param = new
                {
                    questionId
                };
                return conn.GetList<WangdaquestionModel>(where.ToString(), param)?.FirstOrDefault();
            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public void SaveModel(WangdaquestionModel saveModel)
        {
            if (saveModel == null) return;
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                if (saveModel.Id < 1)
                {
                    //新增
                    conn.Insert<long, WangdaquestionModel>(saveModel);
                }
                else
                {
                    //修改
                    conn.Update(saveModel);
                }
            }
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public List<WangdaquestionModel> GetModels(string title)
        {
            var where = new StringBuilder("");
            if (!string.IsNullOrEmpty(title))
            {
                where.Append(" where SearchMate like @SearchMate ");
            }
            var param = new
            {
                SearchMate = "%" + title + "%"
            };
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                return conn.GetList<WangdaquestionModel>(where.ToString(), param)?.ToList();
            }
        }

        /// <summary>
        /// 获取题课比例
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> GetQuestionCourseRate()
        {
            var sqlStr = "select CourseId,CourseName,count(1) CourseCount from (select CourseName,CourseId,Content from wangdaquestion where CourseId is not null group by CourseName,CourseId,Content) a GROUP BY CourseId,CourseName ";
            var result = new Dictionary<string, int>();
            using (var conn = SqlConnectionHelper.GetOpenConnection())
            {
                IEnumerable<dynamic> query = conn.Query(sqlStr);
                foreach (var rows in query)
                {
                    if (!(rows is IDictionary<string, object> fields)) continue;
                    var courseId = fields["CourseId"];
                    var courseCount = fields["CourseCount"];
                    result.Add(courseId.ToString(), DataTypeConvertHelper.ToInt(courseCount));
                }
            }

            return result;
        }
    }
}
