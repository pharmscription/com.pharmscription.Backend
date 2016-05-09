using com.pharmscription.DataAccess.Entities.AddressEntity;
using com.pharmscription.DataAccess.Entities.BaseEntity;

namespace com.pharmscription.DataAccess.Entities.DoctorEntity
{
    using com.pharmscription.DataAccess.Entities.BaseUser;

    public class Doctor: BaseUser
    {
        public Address Address { get; set; }
        public string ZsrNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
    }
}
