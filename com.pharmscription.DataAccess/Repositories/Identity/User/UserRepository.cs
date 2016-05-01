using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using com.pharmscription.DataAccess.Entities.IdentityUserEntity;
using com.pharmscription.DataAccess.Entities.RoleEntity;
using com.pharmscription.DataAccess.Repositories.BaseRepository;
using com.pharmscription.DataAccess.UnitOfWork;
using com.pharmscription.Infrastructure.Exception;

namespace com.pharmscription.DataAccess.Repositories.Identity.User
{
     public class UserRepository : Repository<IdentityUser>, IUserRepository

    {
        public UserRepository(IPharmscriptionUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }

        public Task CreateAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user must not be null");
            }
            return Task.Factory.StartNew(() =>
                    {
                        Add(user);
                        UnitOfWork.Commit();
                    });
        }

        public Task UpdateAsync(IdentityUser user)
        {

            if (user == null)
            {
                throw new ArgumentNullException("user must not be null");
            }
           return Task.Factory.StartNew(() =>
                    {
                        Modify(user);
                        UnitOfWork.Commit();
                    });
        }

        public Task DeleteAsync(IdentityUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user must not be null");
            }
            return Task.Factory.StartNew(() =>
                    {
                        Remove(user);
                        UnitOfWork.Commit();
                    });
        }

        public Task<IdentityUser> FindByIdAsync(Guid userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("userId must not be null");
            }
            return GetAsyncOrThrow(userId);
        }

        public Task<IdentityUser> FindByNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new InvalidArgumentException(userName + " is not valid");
            }
            return Task.Factory.StartNew(() =>
                    {
                        return GetFiltered(user => user.UserName.Equals(userName)).FirstOrDefault();
                    });
        }

        #region PasswordStore
        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash)
        {
            return Task.Factory.StartNew(
                () =>
                    {
                        user.PasswordHash = passwordHash;
                        Modify(user);
                        UnitOfWork.Commit();
                    });
        }

        public Task<string> GetPasswordHashAsync(IdentityUser user)
        {
            return Task.Factory.StartNew(() => Get(user.Id).PasswordHash);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user)
        {
            return Task.Factory.StartNew(() => !string.IsNullOrEmpty(Get(user.Id).PasswordHash));
        }

        #endregion

        #region Role
        public Task AddToRoleAsync(IdentityUser user, string roleName)
        {
            return GetUserWithRolesAsync(user).ContinueWith((t) =>
                    {
                        var u = t.Result;
                        var role = IdentityRole.CreateRole(roleName);
                        u.Roles.Add(role);
                        UnitOfWork.Commit();
                    });
        }

        public Task<IdentityUser> GetUserWithRolesAsync(IdentityUser user)
        {
            return Task.Factory.StartNew(() 
                => GetSet().Where(u => u.Id == user.Id).Include(u => u.Roles).FirstOrDefault());
        }

        public Task RemoveFromRoleAsync(IdentityUser user, string roleName)
        {
            return GetUserWithRolesAsync(user).ContinueWith(
                (t) =>
                    {
                        var u = t.Result;
                        var role = IdentityRole.CreateRole(roleName);
                        u.Roles.Remove(role);
                    });
        }

        public Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            return GetUserWithRolesAsync(user).ContinueWith(
                (t) =>
                    {
                        var u = t.Result;
                        return (IList<string>)u.Roles.Select(role => role.ToString()).ToList();
                    });
        }

        public Task<bool> IsInRoleAsync(IdentityUser user, string roleName)
        {
            return GetUserWithRolesAsync(user).ContinueWith((t) => t.Result.Roles.Contains(IdentityRole.CreateRole(roleName)));
        }
        #endregion
    }
}