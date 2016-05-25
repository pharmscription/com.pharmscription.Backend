using System;

using com.pharmscription.DataAccess.Entities.AddressEntity.CityCodeEntity;
using com.pharmscription.DataAccess.Entities.BaseEntity;
using com.pharmscription.DataAccess.SharedInterfaces;

namespace com.pharmscription.DataAccess.Entities.AddressEntity
{
    public class Address : Entity, ICloneable<Address>, IEquatable<Address>
    {
        public string Street { get; set; }
        public string StreetExtension { get; set; }
        public string State { get; set; }
        public string Number { get; set; }
        public string Location { get; set; }
        public virtual AbstractCityCode CityCode { get; set; }

        public bool Equals(Address other)
        {
            if (other == null)
            {
                return false;
            }
            return CityCode.Equals(other.CityCode) && Location == other.Location && Number == other.Number && State == other.State
                   && Street == other.Street && StreetExtension == other.StreetExtension;
        }

        public Address Clone()
        {
            return new Address
            {
                Street = Street,
                CityCode = CityCode,
                StreetExtension = StreetExtension,
                State = State,
                Number = Number,
                Location = Location
            };
        }
    }
}
