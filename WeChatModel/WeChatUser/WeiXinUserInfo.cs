namespace WeChatModel.WeChatUser
{
    /// <summary>
    /// 用户详细信息
    /// </summary>
    public class WeiXinUserInfo
    {
        /// <summary>
        /// 用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息。
        /// </summary>
        public int Subscribe { get; set; }

        /// <summary>
        /// 用户的标识，对当前公众号唯一
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 用户的昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 用户所在城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 用户所在国家
        /// </summary>
        public string Conuntry { get; set; }

        /// <summary>
        /// 用户所在省份
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 用户的语言，简体中文为zh_CN
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
        /// </summary>
        public string HeadImgUrl { get; set; }

        /// <summary>
        /// 用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
        /// </summary>
        public string Subscribe_Time { get; set; }

        /// <summary>
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段
        /// </summary>
        public string UnionId { get; set; }


    }

    /// <summary>
    /// 只包含OpenId的用户信息
    /// </summary>
    public class WeiXinUserSampleInfo
    {
        /// <summary>
        /// Access_Token
        /// </summary>
        public string Access_Token { get; set; }

        /// <summary>
        /// Expires_In 有效时间
        /// </summary>
        public string Expires_In { get; set; }

        /// <summary>
        /// 刷新token
        /// </summary>
        public string Refresh_Token { get; set; }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 授权方式
        /// </summary>
        public string Scope { get; set; }
    }
}
