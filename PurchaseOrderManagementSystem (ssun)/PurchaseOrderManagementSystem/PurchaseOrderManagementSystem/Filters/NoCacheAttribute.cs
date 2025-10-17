using System;
using System.Web;
using System.Web.Mvc;

namespace PurchaseOrderManagementSystem.Filters
{
    /// <summary>
    /// Prevents the browser from caching pages so that after logout
    /// the back button won't show sensitive data.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var response = filterContext.HttpContext.Response;

            // Disable caching for this response
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.Cache.SetNoStore();
            response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate, no-store, no-cache");

            base.OnResultExecuting(filterContext);
        }
    }
}
