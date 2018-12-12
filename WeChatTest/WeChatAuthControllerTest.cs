using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.CO2NET.Helpers;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MvcExtension;
using WeChatWeb.Controllers.WeChatAuth;

namespace WeChatTest
{
    [TestClass]
    public class WeChatAuthControllerTest
    {
        /// <summary>
        /// 初始化控制器及相关请求参数
        /// </summary>
        /// <param name="xmlFormat"></param>
        protected void Init(string xmlFormat)
        {
            //target = StructureMap.ObjectFactory.GetInstance<WeixinController>();//使用IoC的在这里必须注入，不要直接实例化
            target = new WeChatAuthController();

            inputStream = new MemoryStream();

            var xml = string.Format(xmlFormat, DateTimeHelper.GetUnixDateTime(DateTime.Now));
            var bytes = System.Text.Encoding.UTF8.GetBytes(xml);

            inputStream.Write(bytes, 0, bytes.Length);
            inputStream.Flush();
            inputStream.Seek(0, SeekOrigin.Begin);

            target.SetFakeControllerContext(inputStream);
        }

        /// <summary>
        /// 测试不同类型的请求
        /// </summary>
        /// <param name="xml">微信发过来的xml原文</param>
        protected void PostTest(string xml)
        {
            Init(xml);//初始化

            var timestamp = "itsafaketimestamp";
            var nonce = "whateveryouwant";
            var signature = Senparc.Weixin.MP.CheckSignature.GetSignature(timestamp, nonce, WeChatAuthController.Token);

            DateTime st = DateTime.Now;
            //这里使用MiniPost，绕过日志记录

            var postModel = new PostModel()
            {
                Signature = signature,
                Timestamp = timestamp,
                Nonce = nonce,
            };
            var actual = target.Post(postModel) as FixWeixinBugWeixinResult;
            DateTime et = DateTime.Now;

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Content);

            Console.WriteLine(actual.Content);
            Console.WriteLine("页面用时（ms）：" + (et - st).TotalMilliseconds);
        }

        [TestMethod]
        public void TextPostTest()
        {
            PostTest(string.Format(xmlTextFormat, "TNT2"));
        }

        protected WeChatAuthController target;
        protected Stream inputStream;

        string xmlTextFormat = @"<xml>
    <ToUserName><![CDATA[gh_a96a4a619366]]></ToUserName>
    <FromUserName><![CDATA[olPjZjsXuQPJoV0HlruZkNzKc91E]]></FromUserName>
    <CreateTime>{{0}}</CreateTime>
    <MsgType><![CDATA[text]]></MsgType>
    <Content><![CDATA[{0}]]></Content>
    <MsgId>5832509444155992350</MsgId>
</xml>
";
    }
}
