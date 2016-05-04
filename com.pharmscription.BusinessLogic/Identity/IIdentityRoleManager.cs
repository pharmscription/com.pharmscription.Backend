namespace com.pharmscription.BusinessLogic.Identity
{
    using System;

    using com.pharmscription.DataAccess.Entities.RoleEntity;

    using Microsoft.AspNet.Identity;

    public interface IIdentityRoleManager
    {
        IRoleStore<IdentityRole, Guid> Repository { get; }
    }
}