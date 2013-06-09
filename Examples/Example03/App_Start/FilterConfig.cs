using System.Web;
using System.Web.Mvc;

namespace JQueryFileUpload.Example02
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}