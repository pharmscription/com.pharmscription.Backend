using System;

using com.pharmscription.DataAccess.Entities.IdentityUserEntity;
using com.pharmscription.DataAccess.SharedInterfaces;

using Microsoft.AspNet.Identity;

namespace com.pharmscription.DataAccess.Repositories.Identity.User
{
    public interface IUserRepository : IRepository<IdentityUser>, IUserPasswordStore<IdentityUser, Guid>, IUserRoleStore<IdentityUser, Guid>
    {
         
    }
}