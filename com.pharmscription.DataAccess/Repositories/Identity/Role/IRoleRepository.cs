using System;

using com.pharmscription.DataAccess.Entities.RoleEntity;
using com.pharmscription.DataAccess.SharedInterfaces;

using Microsoft.AspNet.Identity;

namespace com.pharmscription.DataAccess.Repositories.Identity.Role
{
    public interface IRoleRepository : IRepository<IdentityRole>, IRoleStore<IdentityRole, Guid>
    {
         
    }
}