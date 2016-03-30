namespace com.pharmscription.DataAccess.Entities.AddressEntity.CityCodeEntity
{
    public interface ICityCode
    {
        string CityCode { get; set; }
        bool IsValid();
    }
}
