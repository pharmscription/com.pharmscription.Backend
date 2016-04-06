using com.pharmscription.DataAccess.SwissMedic;

namespace com.pharmscription.BusinessLogic.Drug
{
    public class SwissMedicConnector
    {
        public ISwissMedic GetSwissMedicConnection()
        {
            return new SwissMedicMock();
        }
    }
}
