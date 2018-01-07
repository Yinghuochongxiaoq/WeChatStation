#region	Vesion Info
//======================================================================
//Copyright(C) 重庆海外旅业.All right reserved.
//命名空间：WeChatCommon.CustomerAttribute
//文件名称：AuthorizeIgnoreAttribute
//创 建 人：FreshMan
//创建日期：2017/12/30 11:18:05
//用    途：记录类的用途
//======================================================================
#endregion

using System;

namespace WeChatCommon.CustomerAttribute
{
    /// <summary>
    /// 忽略权限
    /// </summary>
    public class AuthorizeIgnoreAttribute:Attribute
    {
        public AuthorizeIgnoreAttribute() { }
    }
}
