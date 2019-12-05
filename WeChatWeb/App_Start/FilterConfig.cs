using System.Web.Mvc;

namespace WeChatWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new Filter.PageAuthFilter());
            filters.Add(new CompressAttribute());
        }
    }
}