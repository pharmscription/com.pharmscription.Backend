using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.pharmscription.Service.Controllers
{
    using System.Net;
    using System.Web.Mvc;

    public class PhaAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            var request = httpContext.Request;
            var response = httpContext.Response;
            var user = httpContext.User;


            if (user.Identity.IsAuthenticated == false)
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
            else
                response.StatusCode = (int)HttpStatusCode.Forbidden;

            response.SuppressFormsAuthenticationRedirect = true;
            response.End();

            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}