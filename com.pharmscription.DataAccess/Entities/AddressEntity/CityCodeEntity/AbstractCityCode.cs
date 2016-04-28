namespace com.pharmscription.DataAccess.Entities.AddressEntity.CityCodeEntity
{
    using BaseEntity;

    public abstract class AbstractCityCode: Entity
    {
        public string CityCode { get; set; }
        public override string ToString()
        {
            return CityCode;
        }
    }
}
