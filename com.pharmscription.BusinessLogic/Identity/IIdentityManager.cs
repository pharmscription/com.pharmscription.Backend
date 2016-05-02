using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.pharmscription.BusinessLogic.Identity
{
    using com.pharmscription.DataAccess.Identity;

    using Microsoft.AspNet.Identity;

    public interface IIdentityManager
    {
        UserManager<MyIdentityUser> UserManager { get; }
        RoleManager<MyIdentityRole> RoleManager { get; }
    }
}
