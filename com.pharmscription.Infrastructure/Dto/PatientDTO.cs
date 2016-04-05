using System;
using System.Runtime.Serialization;

namespace com.pharmscription.Infrastructure.Dto
{
    [DataContract]
    public class PatientDto : BaseDto
    {
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string EMailAddress { get; set; }
        [DataMember]
        public string AhvNumber { get; set; }
        [DataMember]
        public AddressDto Address { get; set; }
        
        public DateTime BirthDate { get; set; }
        [DataMember]
        public string BirthDateStr
        {
            get { return BirthDate.ToString(@"yyyy-MM-dd\THH:mm:ss.fff\Z"); }
            set
            {
                BirthDate=DateTime.ParseExact(value, @"yyyy-MM-dd\THH:mm:ss.fff\Z", System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public string InsuranceNumber { get; set; }
        [DataMember]
        public string Insurance { get; set; }
    }
}
