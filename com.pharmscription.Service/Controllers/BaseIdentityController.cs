using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.pharmscription.Service.Controllers
{
    using System.Security.Claims;
    using System.Web.Mvc;

    using com.pharmscription.BusinessLogic.Identity;
    using com.pharmscription.DataAccess.Entities.BaseUser;
    using com.pharmscription.DataAccess.Entities.PhaIdentiyUser;
    using com.pharmscription.DataAccess.Identity;
    using com.pharmscription.Infrastructure.Constants;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security;

    public class BaseIdentityController : Controller
    {
        private IIdentityManager _identityManager;
        private UserManager<PhaIdentityUser> userManager;
        private RoleManager<IdentityRole> roleManager;

        public BaseIdentityController(IIdentityManager identityManager)
        {
            this._identityManager = identityManager;
            userManager = identityManager.UserManager;
            roleManager = identityManager.RoleManager;
        }

        public BaseUser GetCurrentUser()
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;

            var username = authenticationManager.User?.Identity?.Name;

            var user = userManager.FindByName(username);

            if (user is PhaIdentityUser)
            {
                var phaUser = user as PhaIdentityUser;
                if (phaUser.Roles.Count == 1)
                {
                    var role = this.roleManager.FindById(phaUser.Roles.SingleOrDefault().RoleId);
                    AccountRoles accountRole;
                    AccountRoles.TryParse(role.Name, out accountRole);
                    return _identityManager.GetUser(phaUser.UserId, accountRole);
                }
                else
                {
                    throw new InvalidOperationException("Too many roles");
                }
            }

            return null;
        }

    }
}