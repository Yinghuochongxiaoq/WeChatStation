using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FreshCommonUtility.Web;
using WeChatCommon.Configure;
using WeChatCommon.CustomerAttribute;
using WeChatCommon.LogHelper;
using WeChatCommon.WebHelper;
using WeChatModel.DatabaseModel;
using WeChatModel.Enum;
using WeChatModel.Message;
using WeChatService.Advertise;
using WeChatService.ContentService;
using WeChatService.MessageHandlers;
using WeChatService.SysDicService;

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
            var dicSlider = new SysDicService().GetDicByValue("BlogIndexPageSlider")?.FirstOrDefault();
            var sliderServer = new SysAdvertiseService();
            ViewBag.SliderList = null;
            if (dicSlider != null)
            {
                ViewBag.SliderList = sliderServer.GetList(dicSlider.Id, 1, 10, out _);
            }
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
            ViewBag.Page = page;
            ViewBag.PageCount = total / pageSize + (total % pageSize > 0 ? 1 : 0);
            ViewBag.Url = ViewBag.RootNote + "/Home/BlogHistory";
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
            ViewBag.Page = page;
            ViewBag.PageCount = total / pageSize + (total % pageSize > 0 ? 1 : 0);
            ViewBag.Url = ViewBag.RootNote + "/Home/OpenSourceArea";
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
            ViewBag.Page = page;
            ViewBag.PageCount = total / pageSize + (total % pageSize > 0 ? 1 : 0);
            ViewBag.Url = ViewBag.RootNote + "/Home/Life";
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
                    if (f.Id > model.Id)
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

        /// <summary>
        /// 留言
        /// </summary>
        /// <returns></returns>
        public ActionResult Message()
        {
            var server = new ContentService();
            var topNoList = server.GetTopNoContent(1, 20);
            ViewBag.TopNoList = topNoList;
            return View();
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        public ActionResult CheckCode()
        {
            var yzm = new YzmHelper();
            yzm.CreateImage();
            var code = yzm.Text;
            Session["MessageValidateCode"] = code;
            Bitmap img = yzm.Image;
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return File(ms.ToArray(), @"image/jpeg");
        }

        /// <summary>
        /// 添加留言信息
        /// </summary>
        /// <returns></returns>
        public ActionResult AddCommentInfo()
        {
            var resultMode = new ResponseBaseModel<dynamic>
            {
                ResultCode = ResponceCodeEnum.Fail,
                Message = "响应成功"
            };
            try
            {
                var checkcode = System.Web.HttpContext.Current.GetStringFromParameters("checkcode");
                if (string.IsNullOrEmpty(checkcode) || string.IsNullOrEmpty(Session["MessageValidateCode"]?.ToString()))
                {
                    resultMode.Message = "验证码必填";
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }

                var oldCode = Session["MessageValidateCode"];
                Session["MessageValidateCode"] = null;
                if (!oldCode.Equals(checkcode))
                {
                    resultMode.Message = "验证码错误";
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }
                var content = System.Web.HttpContext.Current.GetStringFromParameters("content");
                var createTime = DateTime.Now;
                var customerEmail = System.Web.HttpContext.Current.GetStringFromParameters("email");
                var customerName = System.Web.HttpContext.Current.GetStringFromParameters("username");
                var customerPhone = System.Web.HttpContext.Current.GetStringFromParameters("tel");
                var fw = new FilterWord();
                string str = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                var filePath = AppConfigurationHelper.GetString("SensitiveFilePath");
                fw.DictionaryPath = str + filePath;
                fw.SourctText = content;
                content = fw.Filter('*');
                if (string.IsNullOrEmpty(content))
                {
                    resultMode.Message = "留言内容不能为空";
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }
                fw.SourctText = customerEmail;
                customerEmail = fw.Filter('*');
                if (string.IsNullOrEmpty(customerEmail) || !RegExp.IsEmail(customerEmail))
                {
                    resultMode.Message = "邮箱内容错误";
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }
                fw.SourctText = customerName;
                customerName = fw.Filter('*');
                if (string.IsNullOrEmpty(customerName))
                {
                    resultMode.Message = "姓名内容错误";
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }
                fw.SourctText = customerPhone;
                customerPhone = fw.Filter('*');
                if (string.IsNullOrEmpty(customerPhone) || !RegExp.IsMobileNo(customerPhone))
                {
                    resultMode.Message = "电话内容错误";
                    return Json(resultMode, JsonRequestBehavior.AllowGet);
                }
                var commentModel = new CustomercommentModel { Content = content, CreateTime = createTime, CustomerName = customerName, CustomerEmail = customerEmail, CustomerPhone = customerPhone, IsDel = FlagEnum.HadZore.GetHashCode(), HasDeal = FlagEnum.HadZore };
                var server = new CustomerCommentService();
                server.SaveModel(commentModel);
                resultMode.Message = "处理成功";
                resultMode.ResultCode = ResponceCodeEnum.Success;
            }
            catch (Exception e)
            {
                LogUtil.LogError(e.Message+e.StackTrace);
                Trace.WriteLine(e);
                resultMode.Message = "系统异常";
            }
            return Json(resultMode, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}