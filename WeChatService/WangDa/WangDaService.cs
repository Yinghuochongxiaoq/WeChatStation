using System.Collections.Generic;
using WeChatDataAccess;
using WeChatModel.DatabaseModel;

namespace WeChatService.WangDa
{
    /// <summary>
    /// 网大数据服务
    /// </summary>
    public class WangDaService
    {
        /// <summary>
        /// 数据服务
        /// </summary>
        private readonly WangDaData _dataAccess = new WangDaData();

        /// <summary>
        /// 根据题目id获取内容信息
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public WangdaquestionModel GetWangDaQuestionModelByQuestionId(string questionId)
        {
            if (string.IsNullOrEmpty(questionId)) return null;
            return _dataAccess.GetWangDaQuestionModelByQuestionId(questionId);
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public void SaveModel(WangdaquestionModel saveModel)
        {
            if (saveModel == null) return;
            _dataAccess.SaveModel(saveModel);
        }

        /// <summary>
        /// 模糊搜索题目或者选项
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public List<WangdaquestionModel> GetModels(string title)
        {
            if (string.IsNullOrEmpty(title)) return null;
            return _dataAccess.GetModels(title);
        }
    }
}
