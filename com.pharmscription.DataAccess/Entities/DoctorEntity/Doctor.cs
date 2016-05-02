using com.pharmscription.DataAccess.Entities.AddressEntity;
using com.pharmscription.DataAccess.Entities.BaseEntity;
using com.pharmscription.DataAccess.Entities.IdentityUserEntity;
using com.pharmscription.DataAccess.SharedInterfaces;

namespace com.pharmscription.DataAccess.Entities.DoctorEntity
{
   public class Doctor : Entity, IIdentityEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
        public string ZsrNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public IdentityUser User { get; set; }
    }
}
