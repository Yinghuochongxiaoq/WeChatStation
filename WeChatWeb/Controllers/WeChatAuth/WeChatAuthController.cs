using System.Web.Mvc;
using Newtonsoft.Json;
using WeChatCommon.CustomerAttribute;
using WeChatCommon.WebHelper;
using WeChatCommon.WeChatAuth;

namespace WeChatWeb.Controllers.WeChatAuth
{
    /// <summary>
    /// 微信认证控制器
    /// </summary>
    public class WeChatAuthController : BaseController
    {
        /// <summary>
        /// 微信授权
        /// </summary>
        /// <returns></returns>
        [AuthorizeIgnore]
        public string Index()
        {
            string token = WeChatConstModel.Token;
            string echoString = HttpContext.GetStringFromParameters("echoStr");
            string signature = HttpContext.GetStringFromParameters("signature");
            string timestamp = HttpContext.GetStringFromParameters("timestamp");
            string nonce = HttpContext.GetStringFromParameters("nonce");
            if (string.IsNullOrEmpty(echoString))
            {
                return "授权失败！";
            }
            bool result = WeChatAuthen.VerifySignature(token, timestamp, nonce, "", signature);
            if (result)
            {
                return echoString;
            }
            return "授权失败！";
        }

        [HttpGet]
        public ActionResult IndexPage()
        {
            ViewBag.UserInfo = JsonConvert.SerializeObject(CurrentModel);
            ViewBag.IsWeChatBrower = IsWeChatBrower;
            return View();
        }
    }
}