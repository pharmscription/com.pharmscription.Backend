
using System.Runtime.Serialization;

namespace com.pharmscription.Infrastructure.Dto
{
    [DataContract]
    public class AddressDto: BaseDto
    {
        [DataMember]
        public string Street { get; set; }
        [DataMember]
        public string StreetExtension { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Number { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string CityCode { get; set; }
    }
}