using System;
using System.Collections.Generic;

using com.pharmscription.DataAccess.Entities.BaseEntity;
using com.pharmscription.DataAccess.Entities.RoleEntity;

using Microsoft.AspNet.Identity;

namespace com.pharmscription.DataAccess.Entities.IdentityUserEntity
{
    public class IdentityUser : Entity, IUser<Guid>
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public virtual ICollection<IdentityRole> Roles { get; set; }

        public string PasswordHash { get; set; } 
    }
}