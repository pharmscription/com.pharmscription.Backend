using com.pharmscription.DataAccess.Insurance;

namespace com.pharmscription.BusinessLogic.Patient
{
    public class InsuranceConnector
    {
        public IInsurance GetInsuranceConnection()
        {
            return Insurance.RealInsurance;
        }
    }
}
