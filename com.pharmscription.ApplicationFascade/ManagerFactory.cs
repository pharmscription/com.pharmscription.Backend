using com.pharmscription.BusinessLogic.Patient;
using com.pharmscription.DataAccess;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.DataAccess.UnitOfWork;

namespace com.pharmscription.ApplicationFascade
{
    public class ManagerFactory
    {
        public IPatientManager PatientManager
        {
            get
            {
                IPharmscriptionUnitOfWork puow = new PharmscriptionDataAccess().UnitOfWork;
                IPatientRepository patientRepository = new PatientRepository(puow);
                return new PatientManager(patientRepository);
            }
        }

    }
}
