using com.pharmscription.BusinessLogic.Drug;
using com.pharmscription.BusinessLogic.Patient;
using com.pharmscription.DataAccess;
using com.pharmscription.DataAccess.Repositories.Drug;
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

        public IDrugManager DrugManager
        {
            get
            {
                IPharmscriptionUnitOfWork puow = new PharmscriptionDataAccess().UnitOfWork;
                IDrugRepository drugRepository = new DrugRepository(puow);
                return new DrugManager(drugRepository);
            }
        }

    }
}
