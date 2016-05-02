using System;
using com.pharmscription.Infrastructure.Exception;

namespace com.pharmscription.BusinessLogic.GuidHelper
{
    public class GuidParser
    {
        public static Guid ParseGuid(string guidValue)
        {
            if (string.IsNullOrWhiteSpace(guidValue))
            {
                throw new InvalidArgumentException("Guidstring was empty");
            }
            Guid guid;
            try
            {
                guid = new Guid(guidValue);
            }
            catch (FormatException)
            {
                throw new InvalidArgumentException("Guidstring was unparsable");
            }
            return guid;
        }
    }
}
