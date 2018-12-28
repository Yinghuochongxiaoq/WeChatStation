using System.Web.Mvc;
using System.Web.Routing;
using FreshCommonUtility.Dapper;
using Senparc.CO2NET;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.MP.MessageHandlers;
using WeChatCommon.LogHelper;
using WeChatService;
using WeChatService.MessageHandlers;
using Config = Senparc.Weixin.Config;

namespace WeChatWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);
            DbLinkTestService.DbLink();
            /* CO2NET 全局注册开始
             * 建议按照以下顺序进行注册
             */

            //设置全局 Debug 状态
            var isGLobalDebug = false;
            var senparcSetting = SenparcSetting.BuildFromWebConfig(isGLobalDebug);

            //CO2NET 全局注册，必须！！
            IRegisterService register = RegisterService.Start(senparcSetting).UseSenparcGlobal(false, null);

            /* 微信配置开始
             * 建议按照以下顺序进行注册
             */

            //设置微信 Debug 状态
            var isWeixinDebug = false;
            var senparcWeixinSetting = SenparcWeixinSetting.BuildFromWebConfig(isWeixinDebug);

            //微信全局注册，必须！！
            register.UseSenparcWeixin(senparcWeixinSetting, senparcSetting);
            //注册AccessToken
            AccessTokenContainer.Register(Config.SenparcWeixinSetting.WeixinAppId, Config.SenparcWeixinSetting.WeixinAppSecret);
            MessageHandler<CustomMessageContext>.GlobalWeixinContext.ExpireMinutes = 3;
            //初始化日志
            LogUtil.Init("");
        }
    }
}
