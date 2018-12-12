using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using WeChatCommon.CustomerAttribute;
using WeChatCommon.WebHelper;
using WeChatCommon.WeChatAuth;
using Senparc.Weixin;
using Senparc.Weixin.MP.MvcExtension;
using WeChatCommon.LogHelper;
using WeChatService.MessageHandlers.CustomMessageHandler;

namespace WeChatWeb.Controllers.WeChatAuth
{
    /// <summary>
    /// 微信认证控制器
    /// </summary>
    public class WeChatAuthController : BaseController
    {
        /// <summary>
        /// 与微信公众账号后台的Token设置保持一致，区分大小写。
        /// </summary>
        public static readonly string Token = Config.SenparcWeixinSetting.Token;

        /// <summary>
        /// 与微信公众账号后台的EncodingAESKey设置保持一致，区分大小写。
        /// </summary>
        public static readonly string EncodingAESKey = Config.SenparcWeixinSetting.EncodingAESKey;

        /// <summary>
        /// 与微信公众账号后台的AppId设置保持一致，区分大小写。
        /// </summary>
        public static readonly string AppId = Config.SenparcWeixinSetting.WeixinAppId;

        /// <summary>
        /// 测试首页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult IndexPage()
        {
            ViewBag.UserInfo = JsonConvert.SerializeObject(CurrentModel);
            ViewBag.IsWeChatBrower = IsWeChatBrower;
            return View();
        }

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

        /// <summary>
        /// 用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML。
        /// v0.8之后的版本可以结合Senparc.Weixin.MP.MvcExtension扩展包，使用WeixinResult，见MiniPost方法。
        /// </summary>
        [AuthorizeIgnore]
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Post(PostModel postModel)
        {
            try
            {
                LogUtil.Log(JsonConvert.SerializeObject(postModel));
                if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, Token))
                {
                    return Content("参数错误！");
                }

                postModel.Token = Token;
                postModel.EncodingAESKey = EncodingAESKey;
                postModel.AppId = AppId;

                //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
                var messageHandler = new CustomMessageHandler(Request.InputStream, postModel);
                //接收消息
                messageHandler.Execute();

                return new FixWeixinBugWeixinResult(messageHandler);
            }
            catch (Exception e)
            {
                LogUtil.Log(e.Message);
            }

            return null;

        }
    }
}