using System;

namespace com.pharmscription.Infrastructure.ExternalDto.InsuranceDto
{
    public class InsurancePatient
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AhvNumber { get; set; }
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string City { get; set; }
        public string CityCode { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string InsuranceNumber { get; set; }
        public string Insurance { get; set; }
    }
}
