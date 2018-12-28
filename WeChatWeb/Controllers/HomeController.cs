using System.Web.Mvc;
using WeChatCommon.Configure;
using WeChatCommon.CustomerAttribute;

namespace WeChatWeb.Controllers
{
    [AuthorizeIgnore]
    public class HomeController : BaseController
    {
        /// <summary>
        ///  GET: Home
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

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
    }
}