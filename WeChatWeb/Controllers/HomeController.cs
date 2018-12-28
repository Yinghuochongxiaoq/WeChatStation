using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        public ActionResult QPage()
        {
            return View();
        }
    }
}