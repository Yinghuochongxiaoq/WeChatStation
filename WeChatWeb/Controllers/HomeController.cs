using System.Collections.Generic;
using System.Web.Mvc;
using WeChatCommon.Configure;
using WeChatCommon.CustomerAttribute;
using WeChatService.ContentService;

namespace WeChatWeb.Controllers
{
    [AuthorizeIgnore]
    public class HomeController : BaseController
    {
        #region [1、结婚请帖]
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <returns></returns>
        public ActionResult QPage()
        {
            return View();
        }

        /// <summary>
        /// 获取所有的信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllMessage()
        {
            var messages = new HunshaMessageServer().GetAllHunshaMessages();
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 插入信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ActionResult InserInfo(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }

            var basePath = Request.PhysicalApplicationPath;
            var sensitive = basePath + AppConfigurationHelper.GetString("SensitiveFilePath");
            var filter = new FilterWord(sensitive);
            filter.SourctText = message;
            message = filter.Filter('*');
            message = message.Replace("*", "");
            if (string.IsNullOrEmpty(message))
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            var info = new HunshaMessage { Message = message };
            new HunshaMessageServer().InsertMessage(info);
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region [2、博客首页]

        /// <summary>
        ///  GET: Home
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 博客列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult BlogHistory(int page = 1, int pageSize = 10)
        {
            var server = new ContentService();
            var contentList = server.GetList(null, null, null, "", null, page, pageSize, out var total);
            ViewBag.Total = total;
            ViewBag.PageCount = total / pageSize + (total % pageSize > 0 ? 1 : 0);
            return View(contentList);
        }

        /// <summary>
        /// 博客详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult BlogDetail(int id)
        {
            return View();
        }
        #endregion
    }
}