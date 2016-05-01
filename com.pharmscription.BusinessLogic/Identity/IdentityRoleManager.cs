using System;

using com.pharmscription.DataAccess.Entities.RoleEntity;

using Microsoft.AspNet.Identity;

namespace com.pharmscription.BusinessLogic.Identity
{
    public class IdentityRoleManager : RoleManager<IdentityRole, Guid>
    {
        public IdentityRoleManager(IRoleStore<IdentityRole, Guid> store)
            : base(store)
        {
        }
    }
}