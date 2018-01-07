#region	Vesion Info
//======================================================================
//Copyright(C) 重庆海外旅业.All right reserved.
//命名空间：WeChatModel.Message
//文件名称：ErrorMessage
//创 建 人：FreshMan
//创建日期：2017/12/30 13:46:25
//用    途：记录类的用途
//======================================================================
#endregion

namespace WeChatModel.Message
{
    public class ErrorMessage
    {
        //{"errcode":40001,"errmsg":"invalid credential"} AppId AppSecret   配置错误，或AccessToken 过期

        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrCode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }

        /// <summary>
        /// 过期
        /// </summary>
        public bool TokenExpired
        {
            get { return ErrCode == "40001"; }
        }
    }
}
