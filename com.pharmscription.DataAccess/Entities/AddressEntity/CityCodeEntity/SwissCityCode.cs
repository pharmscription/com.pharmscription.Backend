using System;
using com.pharmscription.Infrastructure.Exception;
namespace com.pharmscription.DataAccess.Entities.AddressEntity.CityCodeEntity
{
    public class SwissCityCode: AbstractCityCode
    {
        public SwissCityCode()
        {
            
        }
        private SwissCityCode(string cityCode)
        {
            CityCode = cityCode;
        }

        public static SwissCityCode CreateInstance(string cityCode)
        {
            if (IsValid(cityCode))
            {
                return new SwissCityCode(cityCode);
            }
            throw new InvalidArgumentException("City code is not valid");
        }
        public static bool IsValid(string cityCode)
        {
            if (cityCode.Length != 4)
            {
                return false;
            }
            bool isNumeric;
            try
            {
                int n;
                isNumeric = int.TryParse(cityCode, out n);
            }
            catch (FormatException)
            {
                return false;
            }
            return isNumeric;
        }


    }
}
