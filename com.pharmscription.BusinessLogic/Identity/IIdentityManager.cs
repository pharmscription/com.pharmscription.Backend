using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.pharmscription.BusinessLogic.Identity
{
    using System.Security.Claims;

    using com.pharmscription.DataAccess.Entities.BaseUser;
    using com.pharmscription.DataAccess.Entities.PhaIdentiyUser;
    using com.pharmscription.DataAccess.Identity;
    using com.pharmscription.Infrastructure.Constants;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public interface IIdentityManager
    {
        UserManager<PhaIdentityUser> UserManager { get; }

        RoleManager<IdentityRole> RoleManager { get; }

        BaseUser GetUser(Guid id, AccountRoles role);

        ClaimsIdentity GetClaimsIdentity(PhaIdentityUser user);

        IdentityResult CreateAccount(string user, string password, AccountRoles role);

        PhaIdentityUser Find(string userName, string password);
    }
}
