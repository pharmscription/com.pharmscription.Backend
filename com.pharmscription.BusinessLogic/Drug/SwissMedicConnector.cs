namespace com.pharmscription.BusinessLogic.Drug
{
    using DataAccess.SwissMedic;

    public class SwissMedicConnector
    {
        public static ISwissMedic SwissMedicConnection => new SwissMedicMock();
    }
}
