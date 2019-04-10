using System.Collections.Generic;
using System.Linq;
using WeChatDataAccess;
using WeChatModel.DatabaseModel;

namespace WeChatService.ContentService
{
    /// <summary>
    /// 内容服务
    /// </summary>
    public class ContentService
    {
        /// <summary>
        /// 数据服务
        /// </summary>
        private readonly SysContentData _dataAccess = new SysContentData();

        /// <summary>
        /// 获取单个信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Syscontent GetContentModel(long id)
        {
            if (id < 1)
            {
                return null;
            }

            return _dataAccess.GetContentModel(id);
        }

        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="title"></param>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="contentType"></param>
        /// <param name="contentSource"></param>
        /// <param name="indexPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<Syscontent> GetList(string title, string starttime, string endtime, string contentType, string contentSource, int indexPage, int pageSize, out int count)
        {
            count = _dataAccess.GetCount(title, starttime, endtime, contentType, contentSource);
            return _dataAccess.GetModels(title, starttime, endtime, contentType, contentSource, indexPage, pageSize);
        }

        /// <summary>
        /// 获取指定类型的文章
        /// </summary>
        /// <param name="dicValue">配置的字典值（取第一条中的id作为类型）</param>
        /// <param name="count">总数</param>
        /// <param name="page">页号</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public List<Syscontent> GetTypeContents(string dicValue, out int count, int page = 1, int pageSize = 10)
        {
            count = 0;
            if (string.IsNullOrEmpty(dicValue)) return null;
            var dicList = new SysDicService.SysDicService().GetDicByValue(dicValue);
            if (dicList == null || dicList.Count < 1)
            {
                return new List<Syscontent>();
            }
            var contentList = GetList(null, null, null, dicList.FirstOrDefault()?.Id, null, page, pageSize, out count);
            return contentList;
        }

        /// <summary>
        /// 获取TopNo文章
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<Syscontent> GetTopNoContent(int page, int pageSize)
        {
            return _dataAccess.GetTopNoContent(page, pageSize);
        }

        /// <summary>
        /// 获取上一篇，下一篇
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Syscontent> GetPreNextContent(long id)
        {
            if (id < 1) return null;
            return _dataAccess.GetPreNextContent(id);
        }

        /// <summary>
        /// 保存点击数
        /// </summary>
        /// <param name="model"></param>
        public void AddViewinfo(SyscontentviewinfoModel model)
        {
            new SyscontentviewinfoData().SaveMenuModel(model);
        }
    }
}
