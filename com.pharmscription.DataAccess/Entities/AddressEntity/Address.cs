using com.pharmscription.DataAccess.Entities.AddressEntity.CityCodeEntity;
using com.pharmscription.DataAccess.Entities.BaseEntity;

namespace com.pharmscription.DataAccess.Entities.AddressEntity
{
    using SharedInterfaces;
    public class Address: Entity, ICloneable<Address>
    {
        public string Street { get; set; }
        public string StreetExtension { get; set; }
        public string State { get; set; }
        public string Number { get; set; }
        public string Location { get; set; }
        public virtual AbstractCityCode CityCode { get; set; }
        public Address Clone()
        {
            return new Address
            {
                Street =  Street,
                StreetExtension = StreetExtension,
                State = State,
                Number = Number,
                Location = Location,
                CityCode = CityCode.Clone()
            };
        }
    }
}
