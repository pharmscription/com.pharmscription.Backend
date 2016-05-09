using System;
using System.Linq;
using System.Web;
using System.Net;
using System.Security.Claims;
using System.Web.Mvc;

using com.pharmscription.BusinessLogic.Identity;
using com.pharmscription.DataAccess.Identity;
using com.pharmscription.Infrastructure.Dto;

using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace com.pharmscription.Service.Controllers
{
    using System.Collections.Generic;

    using com.pharmscription.DataAccess.Entities.PhaIdentiyUser;
    using com.pharmscription.Infrastructure.Constants;

    using Microsoft.AspNet.Identity.EntityFramework;

    public class AccountController : BaseIdentityController
    {
        private IIdentityManager _manager;
        public AccountController(IIdentityManager manager) : base(manager)
        {
            this._manager = manager;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("account/register")]
        public ActionResult Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                
                var user = this._manager.CreateAccount(model.UserName, model.Password, AccountRoles.Patient);
                if (user != null)
                {
                    return Json(user, JsonRequestBehavior.AllowGet);
                }
                
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("account/login")]
        public ActionResult Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                PhaIdentityUser user = _manager.Find(model.UserName, model.Password);
                if (user != null)
                {
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    ClaimsIdentity identity = this._manager.GetClaimsIdentity(user);
                    AuthenticationProperties props = new AuthenticationProperties { IsPersistent = model.RememberMe };
                    authenticationManager.SignIn(props, identity);

                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
        }

        [Authorize]
        [Route("account/secure")]
        public ActionResult Secure()
        {
            var user = base.GetCurrentUser();
            //IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            return Json(user, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Authorize]
        [Route("account/logout")]
        public ActionResult LogOut()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("account/createtestlogins")]
        public ActionResult CreateTestLogins()
        {
            Dictionary<string, AccountRoles> l = new Dictionary<string, AccountRoles>()
            {
                { "patient", AccountRoles.Patient},
                { "doctor", AccountRoles.Doctor},
                { "drugist", AccountRoles.Drugist},
                { "employee", AccountRoles.DrugstoreEmployee}
            };

            foreach (KeyValuePair<string, AccountRoles> user in l)
            {
                for (int i = 1; i < 11; i++)
                {
                    this._manager.CreateAccount(user.Key+i, user.Key+i, user.Value);
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }


    }
}