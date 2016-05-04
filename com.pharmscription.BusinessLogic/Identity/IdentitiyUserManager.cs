using System;

using com.pharmscription.DataAccess.Entities.IdentityUserEntity;
using com.pharmscription.DataAccess.Repositories.Identity.User;

using Microsoft.AspNet.Identity;

namespace com.pharmscription.BusinessLogic.Identity
{
    public class IdentitiyUserManager : IIdentityUserManager
    {
        public IUserStore<IdentityUser, Guid> Repository { get; }

        public IdentitiyUserManager(IUserRepository repository)
        {
            Repository = repository;
        }
    }
}