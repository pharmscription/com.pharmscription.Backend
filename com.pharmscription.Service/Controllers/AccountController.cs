using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.pharmscription.Service.Controllers
{
    using System.Security.Claims;
    using System.Web.Mvc;
    using System.Web.UI.WebControls;

    using com.pharmscription.BusinessLogic.Identity;
    using com.pharmscription.DataAccess.Identity;
    using com.pharmscription.Infrastructure.Dto;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security;

    public class AccountController : Controller
    {
        private UserManager<MyIdentityUser> userManager;
        private RoleManager<MyIdentityRole> roleManager;

        public AccountController(IIdentityManager manager)
        {
            this.userManager = manager.UserManager;
            this.roleManager = manager.RoleManager;
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
                user.Bio = "bla";

                IdentityResult result = userManager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    var createdUser = userManager.AddToRole(user.Id, "Administrator");
                    return Json(createdUser, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(result.Errors.ToList());
                }

            }
            return Json("Error");
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

                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        
        [Authorize(Roles = "Administrator")]
        [Route("account/secure")]
        public ActionResult Secure()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            return Json(authenticationManager.User?.Identity?.Name, JsonRequestBehavior.AllowGet);

            //return Json("secure", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Authorize]
        [Route("account/logout")]
        public ActionResult LogOut()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return Json("logoueddd..", JsonRequestBehavior.AllowGet);
        }


    }
}