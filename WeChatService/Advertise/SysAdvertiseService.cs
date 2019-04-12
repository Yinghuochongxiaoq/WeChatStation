using System.Collections.Generic;
using WeChatDataAccess;
using WeChatModel.DatabaseModel;

namespace WeChatService.Advertise
{
    public class SysAdvertiseService
    {
        /// <summary>
        /// 数据服务
        /// </summary>
        private SysAdvertiseData _dataAccess = new SysAdvertiseData();

        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="type"></param>
        /// <param name="indexPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<SysadvertisementModel> GetList(string type, int indexPage, int pageSize, out int count)
        {
            count = _dataAccess.GetCount(type);
            return _dataAccess.GetModels(type, indexPage, pageSize);
        }
    }
}
