#region	Vesion Info
//======================================================================
//Copyright(C) 重庆海外旅业.All right reserved.
//命名空间：WeChatCommon.CustomerAttribute
//文件名称：PermissionAttribute
//创 建 人：FreshMan
//创建日期：2017/12/30 14:20:59
//用    途：记录类的用途
//======================================================================
#endregion

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WeChatModel.Enum;

namespace WeChatWeb.Filter
{
    /// <summary>
    /// 用于权限点认证，标记在Action上面
    /// </summary>
    public class PermissionAttribute : FilterAttribute, IActionFilter
    {
        public List<EnumBusinessPermission> Permissions { get; set; }

        public PermissionAttribute(params EnumBusinessPermission[] parameters)
        {
            Permissions = parameters.ToList();
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //throw new NotImplementedException();
        }
    }
}
