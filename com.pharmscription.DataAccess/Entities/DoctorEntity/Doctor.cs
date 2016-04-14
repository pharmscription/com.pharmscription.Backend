using com.pharmscription.DataAccess.Entities.AccountEntity;
using com.pharmscription.DataAccess.Entities.AddressEntity;
using com.pharmscription.DataAccess.Entities.BaseEntity;

namespace com.pharmscription.DataAccess.Entities.DoctorEntity
{
    public class Doctor: EntityWithAccount
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
        public string ZsrNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
    }
}
