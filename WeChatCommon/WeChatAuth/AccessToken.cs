#region	Vesion Info
//======================================================================
//Copyright(C) 重庆海外旅业.All right reserved.
//命名空间：WeChatCommon.WeChatAuth
//文件名称：AccessToken
//创 建 人：FreshMan
//创建日期：2017/12/30 13:26:47
//用    途：记录类的用途
//======================================================================
#endregion

using System;
using Newtonsoft.Json;
using WeChatCommon.WeChatHelper;

namespace WeChatCommon.WeChatAuth
{
    /// <summary>
    /// AccessToken类，公众号通过此token 获取相关信息 （单例类）
    /// </summary>
    public sealed class AccessToken
    {
        private static readonly AccessToken Token = new AccessToken();

        private static readonly object LockObject = new object();

        /// <summary>
        /// 此处 会判断是否过期，没过期返回原存储的Token
        /// </summary>
        public static AccessToken Instance
        {
            get
            {
                lock (LockObject)
                {
                    if (Token.Expired)
                    {
                        lock (LockObject)
                        {
                            if (Token.Expired)
                            {
                                Token.CreateTime = DateTime.Now;
                                Token.CopyModel(JsonConvert.DeserializeObject<AccessToken>(WeiXinHelper.GetAccessToken()));
                            }
                        }
                    }
                }

                return Token;
            }
        }

        /// <summary>
        /// 此处会创建新的Token返回，只有在调用接口提示AccessToken过期时 才调用这个接口。
        /// </summary>
        /// <returns></returns>
        public static AccessToken CreateNewInstance()
        {
            lock (LockObject)
            {
                Token.CreateTime = DateTime.Now;
                Token.CopyModel(JsonConvert.DeserializeObject<AccessToken>(WeiXinHelper.GetAccessToken()));
            }
            return Token;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        private AccessToken()
        {
            CreateTime = DateTime.Now;
            Expires_In = -1;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime;

        /// <summary>
        /// Access_Token
        /// </summary>
        public string Access_Token { get; set; }

        /// <summary>
        /// 有效时间，秒
        /// </summary>
        public int Expires_In { get; set; }

        /// <summary>
        /// 标识是否过期
        /// </summary>
        public bool Expired
        {
            get
            {
                DateTime expiredTime = CreateTime.AddSeconds(Expires_In);

                if (DateTime.Now > expiredTime)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// 复制记录
        /// </summary>
        /// <param name="token"></param>
        public void CopyModel(AccessToken token)
        {
            //token 为空，将 过期时间 设置为 -1
            if (token == null)
            {
                this.Expires_In = -1;
                return;
            }

            this.Access_Token = token.Access_Token;
            this.Expires_In = token.Expires_In;

        }
    }
}
