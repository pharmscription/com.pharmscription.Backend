namespace com.pharmscription.DataAccess.Entities.AddressEntity.CityCodeEntity
{
    using BaseEntity;
    using SharedInterfaces;

    public abstract class AbstractCityCode: Entity, ICloneable<AbstractCityCode>
    {
        public string CityCode { get; set; }
        public abstract AbstractCityCode Clone();

        public override string ToString()
        {
            return CityCode;
        }
    }
}
