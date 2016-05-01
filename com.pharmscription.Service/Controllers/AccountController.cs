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

    public class AccountController : Controller
    {
        private UserManager<MyIdentityUser> userManager;
        private RoleManager<MyIdentityRole> roleManager;

        public AccountController(IIdentityManager manager)
        {
            userManager = manager.UserManager;
            roleManager = manager.RoleManager;
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("account/register")]
        public ActionResult Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                MyIdentityUser user = new MyIdentityUser();

                user.UserName = model.UserName;
                user.BirthDate = DateTime.Now;

                IdentityResult result = userManager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    var createdUser = userManager.AddToRole(user.Id, "Administrator");
                    return Json(createdUser, JsonRequestBehavior.AllowGet);
                }
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
            return new HttpStatusCodeResult(HttpStatusCode.BadGateway);
        }


        [AllowAnonymous]
        [HttpPost]
        [Route("account/login")]
        public ActionResult Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                MyIdentityUser user = userManager.Find(model.UserName, model.Password);
                if (user != null)
                {
                    IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
                    authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    ClaimsIdentity identity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationProperties props = new AuthenticationProperties();
                    props.IsPersistent = model.RememberMe;
                    authenticationManager.SignIn(props, identity);

                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadGateway);
        }
        
        [Authorize(Roles = "Administrator")]
        [Route("account/secure")]
        public ActionResult Secure()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            return Json(authenticationManager.User?.Identity?.Name, JsonRequestBehavior.AllowGet);
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


    }
}