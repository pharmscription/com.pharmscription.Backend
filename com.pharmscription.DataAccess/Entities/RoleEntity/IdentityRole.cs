using System;

using com.pharmscription.DataAccess.Entities.BaseEntity;
using com.pharmscription.Infrastructure.Exception;

using Microsoft.AspNet.Identity;

namespace com.pharmscription.DataAccess.Entities.RoleEntity
{
    public class IdentityRole : Entity, IEquatable<IdentityRole>, IRole<Guid>
    {
        public virtual string Name { get; set; }

        public static IdentityRole CreateRole(string rolename)
        {
            return new IdentityRole { Name = ParseRole(rolename) };
        }

        public bool Equals(IdentityRole other)
        {
            return Name.Equals(other.Name);
        }

        public override bool Equals(object obj)
        {
            // ReSharper disable once UseNullPropagation, since it does not match .NET Equal-Method Guidelines
            if (obj == null)
            {
                return false;
            }
            if (!(obj is IdentityRole))
            {
                return false;
            }
            return Equals((IdentityRole)obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        private static string ParseRole(string rolename)
        {
            string role;
            if (rolename.Equals(RoleType.Doctor))
            {
                role = RoleType.Doctor;
            }
            else if (rolename.Equals(RoleType.DrugStoreEmployee))
            {
                role = RoleType.DrugStoreEmployee;
            }
            else if (rolename.Equals(RoleType.Drugist))
            {
                role = RoleType.Drugist;
            }
            else if (rolename.Equals(RoleType.Patient))
            {
                role = RoleType.Patient;
            }
            else
            {
                throw new InvalidArgumentException("Role " + rolename + " is no valid Role");
            }
            return role;
        }
    }
}