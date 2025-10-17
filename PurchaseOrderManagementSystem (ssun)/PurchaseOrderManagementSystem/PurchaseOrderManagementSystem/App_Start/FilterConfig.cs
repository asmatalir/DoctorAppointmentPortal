using System.Web;
using System.Web.Mvc;
using PurchaseOrderManagementSystem.Filters;

namespace PurchaseOrderManagementSystem
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new NoCacheAttribute());
        }
    }
}
