using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace PurchaseOrderManagementSystem.Controllers
{
    public class BaseController : Controller
    {

        protected int CurrentUserId
        {
            get
            {
                var userIdClaim = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier);
                return userIdClaim != null ? Convert.ToInt32(userIdClaim.Value) : 0;
            }
        }
    }
}   