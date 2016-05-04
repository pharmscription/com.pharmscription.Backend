using System.Runtime.Serialization;

namespace com.pharmscription.Infrastructure.Dto
{
    public class PatientDto : BaseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EMailAddress { get; set; }
        public string AhvNumber { get; set; }
        public AddressDto Address { get; set; }
        public string BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string InsuranceNumber { get; set; }
        public string Insurance { get; set; }
        public IdentityDto Identity { get; set; }
    }
}
