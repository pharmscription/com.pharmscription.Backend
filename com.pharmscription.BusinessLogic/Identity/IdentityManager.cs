using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.pharmscription.BusinessLogic.Identity
{
    using com.pharmscription.DataAccess;
    using com.pharmscription.DataAccess.Identity;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class IdentityManager : IIdentityManager
    {
        private UserManager<MyIdentityUser> userManager;
        private RoleManager<MyIdentityRole> roleManager;

        public IdentityManager(IPharmscriptionDataAccess db)
        {
            //MyIdentityDbContext db = new MyIdentityDbContext();

            UserStore<MyIdentityUser> userStore = new UserStore<MyIdentityUser>(db.IdentityDbContext);
            userManager = new UserManager<MyIdentityUser>(userStore);

            RoleStore<MyIdentityRole> roleStore = new RoleStore<MyIdentityRole>(db.IdentityDbContext);
            roleManager = new RoleManager<MyIdentityRole>(roleStore);

            string roleName = "Administrator";
            if (!roleManager.RoleExists(roleName))
            {
                var roleResult = roleManager.Create(new MyIdentityRole(roleName, roleName));
            }
        }

        public UserManager<MyIdentityUser> UserManager => this.userManager;

        public RoleManager<MyIdentityRole> RoleManager => this.roleManager;
    }
}
