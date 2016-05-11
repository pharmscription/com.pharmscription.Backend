using System;

using com.pharmscription.DataAccess.Entities.BaseEntity;
using com.pharmscription.DataAccess.SharedInterfaces;

namespace com.pharmscription.DataAccess.Entities.AddressEntity.CityCodeEntity
{
    public abstract class AbstractCityCode : Entity, ICloneable<AbstractCityCode>, IEquatable<AbstractCityCode>
    {
        public string CityCode { get; set; }

        public override string ToString()
        {
            return CityCode;
        }
        public abstract bool Equals(AbstractCityCode other);

        public abstract AbstractCityCode Clone();
    }
}
