using System.Web;
using System.Web.Mvc;

namespace CSC_AY2020_Task6
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
