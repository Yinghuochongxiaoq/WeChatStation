using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using WeChatCommon.Configure;
using WeChatCommon.CustomerAttribute;
using WeChatCommon.WebHelper;
using WeChatModel.DatabaseModel;
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
            ViewBag.TechnologyList = GetTypeContents("Technology", out _);
            ViewBag.LifeFeelList = GetTypeContents("FeelLife", out _);
            ViewBag.OpenSourceList = GetTypeContents("OpenSourceArea", out _);
            return View();
        }

        /// <summary>
        /// 博客列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult BlogHistory(int page = 1, int pageSize = 10, string type = null)
        {
            ViewBag.Title = "个人博客日记";
            var contentList = GetTypeContents("Technology", out var total, page, pageSize);
            var topNoList = new ContentService().GetTopNoContent(1, 20);
            ViewBag.TopNoList = topNoList;
            ViewBag.Total = total;
            ViewBag.PageCount = total / pageSize + (total % pageSize > 0 ? 1 : 0);
            return View(contentList);
        }

        /// <summary>
        /// 开源专区
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult OpenSourceArea(int page = 1, int pageSize = 10)
        {
            ViewBag.Title = "开源专区";
            var contentList = GetTypeContents("OpenSourceArea", out var total, page, pageSize);
            var topNoList = new ContentService().GetTopNoContent(1, 20);
            ViewBag.TopNoList = topNoList;
            ViewBag.Total = total;
            ViewBag.PageCount = total / pageSize + (total % pageSize > 0 ? 1 : 0);
            return View("BlogHistory", contentList);
        }

        /// <summary>
        /// 生活
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public ActionResult Life(int page = 1, int pageSize = 10)
        {
            ViewBag.Title = "感悟不断，生活常在";
            var contentList = GetTypeContents("FeelLife", out var total, page, pageSize);
            var topNoList = new ContentService().GetTopNoContent(1, 20);
            ViewBag.TopNoList = topNoList;
            ViewBag.Total = total;
            ViewBag.PageCount = total / pageSize + (total % pageSize > 0 ? 1 : 0);
            return View("BlogHistory", contentList);
        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="dicValue"></param>
        /// <param name="count">总量</param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private List<Syscontent> GetTypeContents(string dicValue, out int count, int page = 1, int pageSize = 10)
        {
            return new ContentService().GetTypeContents(dicValue, out count, page, pageSize);
        }

        /// <summary>
        /// 博客详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult BlogDetail(int id)
        {
            var server = new ContentService();
            var model = server.GetContentModel(id);
            var topNoList = server.GetTopNoContent(1, 20);
            ViewBag.TopNoList = topNoList;
            var preNextList = server.GetPreNextContent(id);
            ViewBag.Pre = null;
            ViewBag.Next = null;
            if (preNextList != null && preNextList.Count > 0)
            {
                preNextList.ForEach(f =>
                {
                    if (f.CreateTime > model.CreateTime)
                    {
                        ViewBag.Next = f;
                    }
                    else
                    {
                        ViewBag.Pre = f;
                    }
                });
            }
            var viewModel = new SyscontentviewinfoModel
            {
                Browser = HttpContext.GetBrowserInfo(),
                ContentId = id,
                CreateTime = DateTime.Now,
                Ip = HttpContext.GetIp()
            };
            ViewBag.ViewCount = server.GetViewCount(id);
            server.AddViewinfo(viewModel);
            return View(model);
        }
        #endregion
    }
}