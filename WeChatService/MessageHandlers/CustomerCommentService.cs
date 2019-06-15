using System;
using System.Collections.Generic;
using WeChatDataAccess;
using WeChatModel.DatabaseModel;
using WeChatModel.Enum;

namespace WeChatService.MessageHandlers
{
    public class CustomerCommentService
    {
        /// <summary>
        /// 数据服务
        /// </summary>
        private CustomerCommentData _dataAccess = new CustomerCommentData();

        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="hasDeal"></param>
        /// <param name="indexPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<CustomercommentModel> GetList(DateTime beginTime, DateTime endTime, FlagEnum hasDeal, int indexPage, int pageSize, out int count)
        {
            count = _dataAccess.GetCount(beginTime, endTime, hasDeal);
            return _dataAccess.GetModels(beginTime, endTime, hasDeal, indexPage, pageSize);
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="saveModel"></param>
        public void SaveModel(CustomercommentModel saveModel)
        {
            _dataAccess.SaveModel(saveModel);
        }
    }
}
