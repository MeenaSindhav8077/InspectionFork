using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inspection.Web.Service
{
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;

            if (session != null && session.IsNewSession)
            {
                string sessionCookie = filterContext.HttpContext.Request.Headers["Cookie"];

                if ((sessionCookie != null) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
                {
                    // Session has expired. Redirect to the login page.
                    filterContext.Result = new RedirectResult("~/Account/Login");
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}