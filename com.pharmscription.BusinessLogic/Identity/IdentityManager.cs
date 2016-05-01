using com.pharmscription.DataAccess;
using com.pharmscription.DataAccess.Identity;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace com.pharmscription.BusinessLogic.Identity
{


    public class IdentityManager : IIdentityManager
    {
        private UserManager<MyIdentityUser> userManager;
        private RoleManager<MyIdentityRole> roleManager;

        public IdentityManager(IPharmscriptionDataAccess db)
        {
            UserStore<MyIdentityUser> userStore = new UserStore<MyIdentityUser>(db.IdentityDbContext);
            userManager = new UserManager<MyIdentityUser>(userStore);

            RoleStore<MyIdentityRole> roleStore = new RoleStore<MyIdentityRole>(db.IdentityDbContext);
            roleManager = new RoleManager<MyIdentityRole>(roleStore);

            string roleName = "Administrator";
            if (!roleManager.RoleExists(roleName))
            {
                 roleManager.Create(new MyIdentityRole(roleName, roleName));
            }
        }

        public UserManager<MyIdentityUser> UserManager => userManager;

        public RoleManager<MyIdentityRole> RoleManager => roleManager;
    }
}
