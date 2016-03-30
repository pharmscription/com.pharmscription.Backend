using System;

namespace com.pharmscription.DataAccess.Entities.AddressEntity.CityCodeEntity
{
    class SwissCityCode: ICityCode
    {
        public string CityCode { get; set; }
        public bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
