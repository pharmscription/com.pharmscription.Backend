
namespace com.pharmscription.Infrastructure.Dto
{
    public class AddressDto: BaseDto
    {
        public string Street { get; set; }
        public string StreetExtension { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string CityCode { get; set; }
    }
}