using System;

namespace com.pharmscription.DataAccess.Entities.AddressEntity.CityCodeEntity
{
    public class SwissCityCode: ICityCode
    {
        public string CityCode { get; set; }
        public bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
