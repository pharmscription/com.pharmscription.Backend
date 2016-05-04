using System;

using com.pharmscription.DataAccess.Entities.IdentityUserEntity;

using Microsoft.AspNet.Identity;

namespace com.pharmscription.BusinessLogic.Identity
{
    public interface IIdentityUserManager
    {
        IUserStore<IdentityUser, Guid> Repository { get; }
    }
}