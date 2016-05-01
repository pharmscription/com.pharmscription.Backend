using System;
using System.Linq;
using System.Threading.Tasks;

using com.pharmscription.DataAccess.Entities.RoleEntity;
using com.pharmscription.DataAccess.Repositories.BaseRepository;
using com.pharmscription.DataAccess.UnitOfWork;

namespace com.pharmscription.DataAccess.Repositories.Identity.Role
{
    public class RoleRepository : Repository<IdentityRole>, IRoleRepository
    {
        public RoleRepository(IPharmscriptionUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }

        public Task CreateAsync(IdentityRole role)
        {
            return Task.Factory.StartNew(() =>
                    {
                        Add(role);
                        UnitOfWork.Commit();
                    });
        }

        public Task UpdateAsync(IdentityRole role)
        {
            return Task.Factory.StartNew(() =>
                    {
                        Modify(role);
                        UnitOfWork.Commit();
                    });
        }

        public Task DeleteAsync(IdentityRole role)
        {
            return Task.Factory.StartNew(
                () =>
                    {
                        Remove(role);
                        UnitOfWork.Commit();
                    });
        }

        public Task<IdentityRole> FindByIdAsync(Guid roleId)
        {
            return GetAsyncOrThrow(roleId);
        }

        public Task<IdentityRole> FindByNameAsync(string roleName)
        {
            return Task.Factory.StartNew(
                () => GetSet().FirstOrDefault(role => role.Name.Equals(roleName)));
        }
    }
}