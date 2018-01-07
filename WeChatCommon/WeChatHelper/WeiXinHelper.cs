#region	Vesion Info
//======================================================================
//Copyright(C) 重庆海外旅业.All right reserved.
//命名空间：WeChatCommon.WeChatHelper
//文件名称：WeiXinHelper
//创 建 人：FreshMan
//创建日期：2017/12/30 13:42:02
//用    途：记录类的用途
//======================================================================
#endregion

using Newtonsoft.Json;
using WeChatCommon.WebHelper;
using WeChatCommon.WeChatAuth;
using WeChatModel.Message;
using WeChatModel.WeChatUser;

namespace WeChatCommon.WeChatHelper
{
    public class WeiXinHelper
    {
        #region [1.获取用户信息]

        /// <summary>
        /// 根据用户Code获取用户信息（包括OpenId的简单信息）
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static WeiXinUserSampleInfo GetUserSampleInfo(string code)
        {
            string url = string.Format(WeChatConstModel.WeiXinUserOpenIdUrl, code);
            WeiXinUserSampleInfo info = HttpClientHelper.GetResponse<WeiXinUserSampleInfo>(url);
            return info;
        }

        /// <summary>
        /// 根据用户Code获取用户信息（包括OpenId的简单信息）
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetUserOpenId(string code)
        {
            return GetUserSampleInfo(code).OpenId;
        }

        /// <summary>
        /// 根据OpenID 获取用户基本信息(需关注公众号）
        /// </summary>
        /// <param name="openId"></param>
        public static WeiXinUserInfo GetUserInfo(string openId)
        {
            if (string.IsNullOrEmpty(openId)) return null;
            var token = AccessToken.Instance;
            string url = string.Format(WeChatConstModel.WeiXinUserGetInfoUrl, token.Access_Token, openId);

            string result = HttpClientHelper.GetResponse(url);

            if (string.IsNullOrEmpty(result))
                return null;

            WeiXinUserInfo info = JsonConvert.DeserializeObject<WeiXinUserInfo>(result);
            //解析用户信息失败，判断 失败Code ，40001 为AccessToken失效，重新创建Token并获取用户信息
            if (info == null || string.IsNullOrEmpty(info.OpenId))
            {
                ErrorMessage msg = JsonConvert.DeserializeObject<ErrorMessage>(result);
                if (msg.TokenExpired)
                {
                    return GetUserInfoByNewAccessToken(openId);
                }
            }

            return info;
        }

        /// <summary>
        /// 创建新的AccessToken 并获取用户信息
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        private static WeiXinUserInfo GetUserInfoByNewAccessToken(string openId)
        {
            var token = AccessToken.CreateNewInstance();
            string url = string.Format(WeChatConstModel.WeiXinUserGetInfoUrl, token.Access_Token, openId);
            WeiXinUserInfo info = HttpClientHelper.GetResponse<WeiXinUserInfo>(url);
            return info;
        }

        /// <summary>
        /// 获取Access_Token
        /// </summary>
        /// <returns></returns>
        public static string GetAccessToken()
        {
            string url = WeChatConstModel.WeiXinAccessTokenUrl;
            string result = HttpClientHelper.GetResponse(url);
            return result;
        }
        #endregion
    }
}
