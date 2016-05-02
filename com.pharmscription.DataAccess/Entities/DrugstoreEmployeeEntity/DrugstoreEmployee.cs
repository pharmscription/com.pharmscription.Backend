using com.pharmscription.DataAccess.Entities.BaseEntity;
using com.pharmscription.DataAccess.Entities.IdentityUserEntity;
using com.pharmscription.DataAccess.SharedInterfaces;

namespace com.pharmscription.DataAccess.Entities.DrugstoreEmployeeEntity
{
    public class DrugstoreEmployee : Entity, IIdentityEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IdentityUser User { get; set; }
    }
}