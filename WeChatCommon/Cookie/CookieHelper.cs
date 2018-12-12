﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WeChatCommon.Cookie
{
    public class CookieHelper
    {
        /// <summary>
        /// 是否Ip
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool IsIp(string s)
        {
            string text1 = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$";
            return Regex.IsMatch(s, text1);
        }

        /// <summary>
        /// 获取域名
        /// </summary>
        private static string ServerDomain
        {
            get
            {
                string urlHost = HttpContext.Current.Request.Url.Host.ToLower();
                string[] urlHostArray = urlHost.Split('.');
                if (urlHostArray.Length < 3 || IsIp(urlHost))
                {
                    return urlHost;
                }
                string urlHost2 = urlHost.Remove(0, urlHost.IndexOf(".", StringComparison.Ordinal) + 1);
                if ((urlHost2.StartsWith("com.") || urlHost2.StartsWith("net.")) || (urlHost2.StartsWith("org.") || urlHost2.StartsWith("gov.")))
                {
                    return urlHost;
                }
                return urlHost2;
            }
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static HttpCookie GetCookies(string cookieName)
        {
            return HttpContext.Current.Request.Cookies[cookieName];
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string GetCookie(string cookieName)
        {
            var cookie = GetCookies(cookieName);
            return cookie != null ? cookie.Value : string.Empty;
        }

        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="valueName"></param>
        /// <returns></returns>
        public static string GetCookie(string cookieName, string valueName)
        {
            var cookie = GetCookies(cookieName);
            return cookie != null ? cookie.Values[valueName] : string.Empty;
        }

        /// <summary>
        /// 添加一个Cookie（24小时过期）
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="cookieValue"></param>
        public static void SetCookie(string cookieName, string cookieValue)
        {
            SetCookie(cookieName, cookieValue, DateTime.Now.AddDays(1.0));
        }

        /// <summary>
        /// 添加一个Cookie
        /// </summary>
        /// <param name="cookieName">cookie名</param>
        /// <param name="cookieValue">cookie值</param>
        /// <param name="expires">过期时间 DateTime</param>
        /// <param name="domain"></param>
        public static void SetCookie(string cookieName, string cookieValue, DateTime expires, string domain = null)
        {
            if (string.IsNullOrEmpty(domain))
            {
                domain = ServerDomain;
            }
            string urlHost = HttpContext.Current.Request.Url.Host.ToLower();
            var cookie = new HttpCookie(cookieName)
            {
                Value = cookieValue,
                Expires = expires,
                Domain = string.IsNullOrEmpty(domain) ? urlHost : domain
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 设置2级Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="cookieValues"></param>
        /// <param name="domain"></param>
        /// <param name="expires"></param>
        public static void SetCookies(string cookieName, List<KeyValuePair<string, string>> cookieValues, string domain = null, DateTime? expires = null)
        {
            if (cookieValues == null || !cookieValues.Any())
            {
                return;
            }
            var cookie = GetCookies(cookieName) ?? new HttpCookie(cookieName);
            foreach (var item in cookieValues.Where(item => !string.IsNullOrEmpty(item.Key)))
            {
                cookie.Values[item.Key] = item.Value;
            }
            if (expires != null)
            {
                cookie.Expires = expires.Value;
            }

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 删除cookies
        /// </summary>
        /// <param name="name"></param>
        public static void DelCookies(string name)
        {
            if (HttpContext.Current.Request.Cookies.AllKeys.Length < 1)
            {
                return;
            }
            HttpCookie cookies = HttpContext.Current.Request.Cookies[name];
            if (cookies != null)
            {
                cookies.Expires = DateTime.Today.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookies);
                HttpContext.Current.Request.Cookies.Remove(name);
            }
        }
    }

}
