using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace WeChatCommon.WebHelper
{
    public static class WebRequestHelper
    {
        /// <summary>
        /// 验证字符串是否为数字（正则表达式）（true = 是数字, false = 不是数字）
        /// </summary>
        /// <param name="validatedString">被验证的字符串</param>
        /// <returns>true = 是数字, false = 不是数字</returns>
        private static bool IsNumeric(string validatedString)
        {
            const string numericPattern = @"^[-]?\d+[.]?\d*$";
            return Regex.IsMatch(validatedString, numericPattern);
        }

        /// <summary>
        /// Get Querystring or Request.From params,you also can define params in method param use [FromBody]Type params string.
        /// </summary>
        /// <param name="context">request context</param>
        /// <param name="key">params key</param>
        /// <returns></returns>
        public static string GetStringFromParameters(this HttpContextBase context, string key)
        {
            var value = string.Empty;
            if (string.IsNullOrEmpty(key) || context == null)
            {
                return value;
            }
            value = context.Request.QueryString[key];
            if (string.IsNullOrEmpty(value)) value = context.Request.Form[key];
            return value;
        }

        /// <summary>
        /// Get Querystring or Request.From params,you also can define params in method param use [FromBody]Type params int.
        /// </summary>
        /// <param name="context">request context</param>
        /// <param name="key">params key</param>
        /// <returns></returns>
        public static int GetIntFromParameters(this HttpContextBase context, string key)
        {
            var value = default(int);
            if (!string.IsNullOrEmpty(key) && context != null)
            {
                var stringValue = context.Request.QueryString[key];
                if (string.IsNullOrEmpty(stringValue)) stringValue = context.Request.Form[key];
                value = (!string.IsNullOrEmpty(stringValue)
                         && IsNumeric(stringValue))
                            ? int.Parse(stringValue)
                            : default(int);
            }
            return value;
        }

        /// <summary>
        /// Get Querystring params of DateTime type.
        /// </summary>
        /// <param name="context">request context</param>
        /// <param name="key">params key</param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromParameters(this HttpContextBase context, string key)
        {
            var value = new DateTime(1900, 01, 01);
            if (string.IsNullOrEmpty(key) || context == null)
            {
                return value;
            }
            var stringValue = context.Request.QueryString[key];
            if (string.IsNullOrEmpty(stringValue)) stringValue = context.Request.Form[key];
            return !DateTime.TryParse(stringValue, out value) ? new DateTime(1900, 1, 1) : value;
        }

        /// <summary>
        /// 获取ip地址
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetIp(this HttpContextBase context)
        {
            string ipv4 = String.Empty;
            if (context.Request.UserHostAddress != null)
                foreach (IPAddress ipa in Dns.GetHostAddresses(context.Request.UserHostAddress))
                {
                    if (ipa.AddressFamily.ToString() == "InterNetwork")
                    {
                        ipv4 = ipa.ToString();
                        break;
                    }
                }

            if (ipv4 != String.Empty)
            {
                return ipv4;
            }
            foreach (IPAddress ipa in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (ipa.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4 = ipa.ToString();
                    break;
                }
            }
            return ipv4;
        }

        /// <summary>
        /// 获取浏览器信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetBrowserInfo(this HttpContextBase context)
        {
            var browser = context.Request.Browser.Browser + context.Request.Browser.Version;//获取客户端浏览器的完整版本号   
            var plat = context.Request.Browser.Platform;//获取客户端使用平台的名字
            return plat + "  " + browser;
        }
    }
}
