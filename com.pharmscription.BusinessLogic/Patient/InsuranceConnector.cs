using com.pharmscription.DataAccess.Insurance;

namespace com.pharmscription.BusinessLogic.Patient
{
    public class InsuranceConnector
    {
        public static IInsurance InsuranceConnection => Insurance.RealInsurance;
    }
}
