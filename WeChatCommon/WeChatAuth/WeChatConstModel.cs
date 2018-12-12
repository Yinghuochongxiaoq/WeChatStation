using WeChatCommon.Configure;

namespace WeChatCommon.WeChatAuth
{
    /// <summary>
    /// 应用账号配置
    /// </summary>
    public static class WeChatConstModel
    {
        /// <summary>
        /// AppId
        /// </summary>
        public static readonly string AppId = AppConfigurationHelper.GetString("WeixinAppId");

        /// <summary>
        /// Appsecret
        /// </summary>
        public static readonly string Appsecret = AppConfigurationHelper.GetString("WeixinAppSecret");

        /// <summary>
        /// Token
        /// </summary>
        public static readonly string Token = AppConfigurationHelper.GetString("WeixinToken");

        /// <summary>
        /// 是否静默授权 默认：false:非静默授权;true:静默授权
        /// </summary>
        public static readonly bool IsSilentAuthorization = AppConfigurationHelper.GetString("IsSilentAuthorization").Equals("true", System.StringComparison.CurrentCultureIgnoreCase);

        /// <summary>
        /// 公众号获取AccessToken的Url(需Format  0.AppId 1.Secret)
        /// </summary>
        private const string AccessTokenUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";

        /// <summary>
        /// 公众号获取Token的Url
        /// </summary>
        public static string WeiXinAccessTokenUrl { get { return string.Format(AccessTokenUrl, AppId, Appsecret); } }

        /// <summary>
        /// 根据Code 获取用户OpenId Url
        /// </summary>
        private const string UserGetOpenIdUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";

        /// <summary>
        /// 根据Code 获取用户OpenId的Url 需要Format 0.code
        /// </summary>
        public static string WeiXinUserOpenIdUrl { get { return string.Format(UserGetOpenIdUrl, AppId, Appsecret, "{0}"); } }

        /// <summary>
        /// 根据OpenId 获取用户基本信息 Url（需要Format0.access_token 1.openid）
        /// </summary>
        public const string WeiXinUserGetInfoUrl = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN";

        /// <summary>
        /// OAuth2 静默授权Url，需要Format0.AppId  1.Uri  2.state
        /// </summary>
        private const string OAuth2BaseUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect";

        /// <summary>
        /// OAuth2 非静默授权Url，需要Format0.AppId  1.Uri
        /// </summary>
        private const string OAuth2UserinfoUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";
        /// <summary>
        /// OAuth2 静默授权Url，需要Format  0.Uri
        /// </summary>
        public static string WeiXinUserOAuth2Url { get { return string.Format(OAuth2BaseUrl, AppId, "{0}", "STATE"); } }

        /// <summary>
        /// OAuth2 非静默授权Url，需要Format  0.Uri
        /// </summary>
        public static string WeiXinDetailUserOAuth2Url { get { return string.Format(OAuth2UserinfoUrl, AppId, "{0}"); } }

        /// <summary>
        /// 创建获取QrCode的Ticket Url  需要Format 0 access_token
        /// </summary>
        public const string WeiXinTicketCreateUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";

        /// <summary>
        /// 获取二维码图片Url,需要Format 0.ticket
        /// </summary>
        public const string WeiXinQrCodeGetUrl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}";

        /// <summary>
        /// 创建菜单Url 需要Format 0.access_token
        /// </summary>
        public const string WeiXinMenuCreateUrl = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";

        /// <summary>
        /// 获取菜单Url 需要Format 0.access_token
        /// </summary>
        public const string WeiXinMenuGetUrl = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}";

        /// <summary>
        /// 删除菜单Url 需要Format 0.access_token
        /// </summary>
        public const string WeiXinMenuDeleteUrl = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}";

        /// <summary>
        /// 生成预支付账单Url ，需替换 0 access_token
        /// </summary>
        public const string WeiXinPayPrePayUrl = "https://api.weixin.qq.com/pay/genprepay?access_token={0}";

        /// <summary>
        /// 订单查询Url ，需替换0 access_token
        /// </summary>
        public const string WeiXinPayOrderQueryUrl = "https://api.weixin.qq.com/pay/orderquery?access_token={0}";

        /// <summary>
        /// 发货通知Url，需替换 0 access_token
        /// </summary>
        public const string WeiXinPayDeliverNotifyUrl = "https://api.weixin.qq.com/pay/delivernotify?access_token={0}";

        /// <summary>
        /// 统一预支付Url
        /// </summary>
        public const string WeiXinPayUnifiedPrePayUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        /// <summary>
        /// 订单查询Url
        /// </summary>
        public const string WeiXinPayUnifiedOrderQueryUrl = "https://api.mch.weixin.qq.com/pay/orderquery";

        /// <summary>
        /// 退款申请Url
        /// </summary>
        public const string WeiXinPayUnifiedOrderRefundUrl = "https://api.mch.weixin.qq.com/secapi/pay/refund";

        /// <summary>
        /// 获取二维码 所需Ticket 需要上传的Json字符串（需要Format 0.scene_id）
        /// </summary>
        /// <remarks>scene_id场景值ID  永久二维码时最大值为100000（目前参数只支持1--100000）</remarks>
        public const string WeiXinQrCodeTicketCreateJsonString = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\":{0}}}}";

        /// <summary>
        /// 本地iis端口
        /// </summary>
        public static readonly string LocalIISPart = AppConfigurationHelper.GetString("LocalIISPart");
    }
}
