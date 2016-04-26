using System;
using com.pharmscription.Infrastructure.Exception;

namespace com.pharmscription.BusinessLogic.GuidHelper
{
    public class GuidParser
    {
        public static Guid ParseGuid(string guidString)
        {
            if (string.IsNullOrWhiteSpace(guidString))
            {
                throw new InvalidArgumentException("Guidstring was empty");
            }
            Guid guid;
            try
            {
                guid = new Guid(guidString);
            }
            catch (FormatException)
            {
                throw new InvalidArgumentException("Guidstring was unparsable");
            }
            return guid;
        }
    }
}
