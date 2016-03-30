using System;

namespace com.pharmscription.DataAccess.Entities.AddressEntity.CityCodeEntity
{
    public class SwissCityCode: ICityCode
    {
        public string CityCode { get; set; }
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
            throw new ArgumentException("City code is not valid");
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
