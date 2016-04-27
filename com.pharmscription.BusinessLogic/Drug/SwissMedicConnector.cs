namespace com.pharmscription.BusinessLogic.Drug
{
    using DataAccess.SwissMedic;

    public class SwissMedicConnector
    {
        public ISwissMedic GetSwissMedicConnection()
        {
            return new SwissMedicMock();
        }
    }
}
