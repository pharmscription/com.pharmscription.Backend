namespace com.pharmscription.BusinessLogic.Drug
{
    using com.pharmscription.DataAccess.SwissMedic;

    public class SwissMedicConnector
    {
        public ISwissMedic GetSwissMedicConnection()
        {
            return new SwissMedicMock();
        }
    }
}
