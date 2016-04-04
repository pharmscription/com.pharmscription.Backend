using com.pharmscription.DataAccess.Entities.AddressEntity.CityCodeEntity;
using com.pharmscription.DataAccess.Entities.BaseEntity;

namespace com.pharmscription.DataAccess.Entities.AddressEntity
{
    public class Address: Entity
    {
        public string Street { get; set; }
        public string StreetExtension { get; set; }
        public string State { get; set; }
        public string Number { get; set; }
        public string Location { get; set; }
        public ICityCode CityCode { get; set; }
    }
}
