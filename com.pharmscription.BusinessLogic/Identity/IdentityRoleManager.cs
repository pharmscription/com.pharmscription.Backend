using System;

using com.pharmscription.DataAccess.Entities.RoleEntity;

using Microsoft.AspNet.Identity;

namespace com.pharmscription.BusinessLogic.Identity
{
    using com.pharmscription.DataAccess.Repositories.Identity.Role;

    public class IdentityRoleManager : IIdentityRoleManager
    {
        public IRoleStore<IdentityRole, Guid> Repository { get; }

        public IdentityRoleManager(IRoleRepository repository)
        {
            Repository = repository;
        }

    }
}