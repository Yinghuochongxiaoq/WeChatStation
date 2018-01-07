#region	Vesion Info
//======================================================================
//Copyright(C) 重庆海外旅业.All right reserved.
//命名空间：WeChatModel.WeChatUser
//文件名称：WeChatSystemUserInfo
//创 建 人：FreshMan
//创建日期：2017/12/30 14:28:28
//用    途：记录类的用途
//======================================================================
#endregion

using System.Collections.Generic;
using WeChatModel.Enum;

namespace WeChatModel.WeChatUser
{
    /// <summary>
    /// 
    /// </summary>
    public class WeChatSystemUserInfo
    {
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
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段
        /// </summary>
        public string UnionId { get; set; }

        /// <summary>
        /// 权限列表
        /// </summary>
        public List<EnumBusinessPermission> BusinessPermissionList { get; set; }
    }
}
