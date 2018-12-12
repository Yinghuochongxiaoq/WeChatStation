using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace WeChatCommon.WeChatAuth
{
    /// <summary>
    /// 微信认证
    /// </summary>
    public class WeChatAuthen
    {
        /// <summary>
        /// 授权认证
        /// </summary>
        /// <param name="sToken">Token</param>
        /// <param name="sTimeStamp">时间戳</param>
        /// <param name="sNonce">nonce随机参数</param>
        /// <param name="sMsgEncrypt">加密串</param>
        /// <param name="sSigture">签名</param>
        /// <returns>true:签名正确；false:签名错误</returns>
        public static bool VerifySignature(string sToken, string sTimeStamp, string sNonce, string sMsgEncrypt, string sSigture)
        {
            string hash = "";
            var ret = GenarateSinature(sToken, sTimeStamp, sNonce, sMsgEncrypt, ref hash);
            if (ret != 0)
                return false;
            if (hash == sSigture)
                return true;
            return false;
        }

        /// <summary>
        /// 计算加密值
        /// </summary>
        /// <param name="sToken">Token值</param>
        /// <param name="sTimeStamp">时间戳</param>
        /// <param name="sNonce">nonce随机参数</param>
        /// <param name="sMsgEncrypt">加密私钥</param>
        /// <param name="sMsgSignature">返回计算得出的签名</param>
        /// <returns></returns>
        private static int GenarateSinature(string sToken, string sTimeStamp, string sNonce, string sMsgEncrypt, ref string sMsgSignature)
        {
            ArrayList al = new ArrayList { sToken, sTimeStamp, sNonce };
            if (!string.IsNullOrEmpty(sMsgEncrypt))
            {
                al.Add(sMsgEncrypt);
            }
            al.Sort(new DictionarySort());
            string raw = "";
            for (int i = 0; i < al.Count; ++i)
            {
                raw += al[i];
            }

            string hash;
            try
            {
                SHA1 sha = new SHA1CryptoServiceProvider();
                var enc = new ASCIIEncoding();
                byte[] dataToHash = enc.GetBytes(raw);
                byte[] dataHashed = sha.ComputeHash(dataToHash);
                hash = BitConverter.ToString(dataHashed).Replace("-", "");
                hash = hash.ToLower();
            }
            catch (Exception)
            {
                return -1;
            }
            sMsgSignature = hash;
            return 0;
        }
    }
}
