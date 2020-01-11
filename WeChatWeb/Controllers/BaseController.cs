using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FreshCommonUtility.Configure;
using FreshCommonUtility.DataConvert;
using Newtonsoft.Json;
using WeChatCommon.Cookie;
using WeChatCommon.CustomerAttribute;
using WeChatCommon.WebHelper;
using WeChatCommon.WeChatAuth;
using WeChatCommon.WeChatHelper;
using WeChatModel.Config;
using WeChatModel.WeChatUser;
using WeChatWeb.Filter;

namespace WeChatWeb.Controllers
{
    /// <summary>
    /// 基类控制器
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// 方法执行前
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            #region [1、验证是否在服务时间内]

            var startTimeStr = AppConfigurationHelper.GetString("SystemRunStartTime");
            var endTimeStr = AppConfigurationHelper.GetString("SystemRunEndTime");
            //没有配置时间
            if (!string.IsNullOrEmpty(startTimeStr) && !string.IsNullOrEmpty(endTimeStr))
            {
                var startTime = DataTypeConvertHelper.ToDateTime(startTimeStr);
                var endTime = DataTypeConvertHelper.ToDateTime(endTimeStr);

                if (startTime <= new DateTime(1900, 1, 1) || endTime <= new DateTime(1900, 1, 1))
                {
                    filterContext.Result = Request.UrlReferrer != null ? Stop("系统运行时间配置错误！", Request.UrlReferrer.AbsoluteUri) : Content("系统运行时间配置错误！");
                    return;
                }
                startTime = new DateTime(1900, 1, 1, startTime.Hour, startTime.Minute, startTime.Second);
                endTime = new DateTime(1900, 1, 1, endTime.Hour, endTime.Minute, endTime.Second);
                var newTime = DateTime.Now;
                newTime = new DateTime(1900, 1, 1, newTime.Hour, newTime.Minute, newTime.Second);
                if (newTime < startTime || newTime > endTime)
                {
                    filterContext.Result = Request.UrlReferrer != null ? Stop("系统处于维护期！", Request.UrlReferrer.AbsoluteUri) : Content("系统处于维护期！");
                    return;
                }
            }

            #endregion

            #region [2、验证权限]

            var noAuthorizeAttributesController = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AuthorizeIgnoreAttribute), false);
            if (noAuthorizeAttributesController.Length > 0) return;
            var noAuthorizeAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AuthorizeIgnoreAttribute), false);
            if (noAuthorizeAttributes.Length > 0) return;

            base.OnActionExecuting(filterContext);

            //验证是否登陆或者拿到了授权
            if (CurrentModel == null)
            {
                filterContext.Result = RedirectToAction("Index", "WeChatAuth");
                var code = GetCode(filterContext);
                var openId = WeiXinHelper.GetUserOpenId(code);
                var wechatUserInfo = WeiXinHelper.GetUserInfo(openId);

                if (wechatUserInfo == null || (string.IsNullOrEmpty(wechatUserInfo.UnionId) && string.IsNullOrEmpty(wechatUserInfo.OpenId)))
                {
                    //openId获取失败，跳转到错误页
                }
                else
                {
                    var systemUserInfo = new WeChatSystemUserInfo
                    {
                        NickName = wechatUserInfo.NickName,
                        UnionId = wechatUserInfo.UnionId,
                        OpenId = wechatUserInfo.OpenId,
                        Sex = wechatUserInfo.Sex
                    };
                    //TODO 处理权限
                    //设置用户信息到cookie中
                    CookieHelper.SetCookie(ConfigModel.SystemUserInfo, JsonConvert.SerializeObject(systemUserInfo));
                }
            }

            //权限验证
            var permissionAttributes = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(PermissionAttribute), false).Cast<PermissionAttribute>();
            permissionAttributes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(PermissionAttribute), false).Cast<PermissionAttribute>().Union(permissionAttributes);
            var attributes = permissionAttributes as IList<PermissionAttribute> ?? permissionAttributes.ToList();
            if (attributes.Any())
            {
                var hasPermission = true;
                foreach (var attr in attributes)
                {
                    if (attr.Permissions.Any(permission => !CurrentModel.BusinessPermissionList.Contains(permission)))
                    {
                        hasPermission = false;
                    }
                }

                if (!hasPermission)
                {
                    filterContext.Result = Request.UrlReferrer != null ? Stop("没有权限！", Request.UrlReferrer.AbsoluteUri) : Content("没有权限！");
                }
            }
            #endregion

        }

        /// <summary>
        /// 转向到一个提示页面，然后自动返回指定的页面
        /// </summary>
        /// <param name="notice"></param>
        /// <param name="redirect"></param>
        /// <param name="isAlert"></param>
        /// <returns></returns>
        public ContentResult Stop(string notice, string redirect, bool isAlert = false)
        {
            var content = "<meta http-equiv='refresh' content='1;url=" + redirect + "' /><body style='margin-top:0px;color:red;font-size:24px;'>" + notice + "</body>";

            if (isAlert)
                content = string.Format("<script>alert('{0}'); window.location.href='{1}'</script>", notice, redirect);

            return Content(content);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        public WeChatSystemUserInfo CurrentModel
        {
            get
            {
                var userInfoStr = CookieHelper.GetCookie(ConfigModel.SystemUserInfo);
                if (!string.IsNullOrEmpty(userInfoStr))
                {
                    var userModel = JsonConvert.DeserializeObject<WeChatSystemUserInfo>(userInfoStr);
                    return userModel;
                }
                return null;
            }
        }

        /// <summary>
        /// 是否在微信浏览器中打开
        /// </summary>
        public bool IsWeChatBrower
        {
            get
            {
                string userAgent = Request.UserAgent;
                if (userAgent != null && userAgent.ToLower().Contains("micromessenger"))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 获取code代码
        /// </summary>
        /// <returns></returns>
        private static string GetCode(ActionExecutingContext filterContext)
        {
            var code = filterContext.RequestContext.HttpContext.GetStringFromParameters("Code");
            if (!string.IsNullOrEmpty(code))  //判断code是否存在
            {
                var cookieCode = CookieHelper.GetCookie("Code");
                if (string.IsNullOrEmpty(cookieCode))  //判断是否是第二次进入
                {
                    CookieHelper.SetCookie("Code", code, DateTime.Now.AddDays(1));  //写code 保存到cookies
                }
                else
                {
                    CookieHelper.DelCookies("code"); //删除cookies
                    CodeUrl(filterContext);//code重新跳转URL
                }
            }
            else
            {
                CodeUrl(filterContext);//code跳转URL
            }
            return code;
        }

        /// <summary>
        /// 跳转codeURL
        /// </summary>
        /// <param name="filterContext">当前请求</param>
        private static void CodeUrl(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Url != null)
            {
                var currentUrl = filterContext.HttpContext.Request.Url.AbsoluteUri;
                if (!string.IsNullOrEmpty(WeChatConstModel.LocalIISPart))
                {
                    if (currentUrl.Contains(WeChatConstModel.LocalIISPart))
                    {
                        currentUrl = currentUrl.Replace($":{WeChatConstModel.LocalIISPart}", "");
                    }
                }
                var changeEncodeUrl = HttpUtility.UrlEncode(currentUrl);
                var url = string.Format(
                    WeChatConstModel.IsSilentAuthorization
                        ? WeChatConstModel.WeiXinUserOAuth2Url
                        : WeChatConstModel.WeiXinDetailUserOAuth2Url, changeEncodeUrl);
                filterContext.Result = new RedirectResult(url);
            }
        }

        /// <summary>
        /// 重写初始化方法
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            InitializeStaticResource();

        }

        /// <summary>
        /// 设置全局变量
        /// </summary>
        private void InitializeStaticResource()
        {
            ViewBag.RootNode = AppConfigurationHelper.GetString("ReferenceKey.RootNode", "") ?? string.Empty;
        }
    }
}