using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreshCommonUtility.Dapper;
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
    }
}
