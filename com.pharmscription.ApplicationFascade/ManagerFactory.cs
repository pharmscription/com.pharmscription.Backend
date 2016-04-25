using com.pharmscription.BusinessLogic.Drug;
using com.pharmscription.BusinessLogic.Patient;
using com.pharmscription.DataAccess.Repositories.Drug;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.BusinessLogic.Prescription;
using com.pharmscription.DataAccess.Repositories.Prescription;

namespace com.pharmscription.ApplicationFascade
{
    public class ManagerFactory
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IDrugRepository _drugRepository;
        private readonly IPrescriptionRepository _prescriptionRepository;
        public ManagerFactory(IPatientRepository patientRepository, IDrugRepository drugRepository, IPrescriptionRepository prescriptionRepository)
        {
            _patientRepository = patientRepository;
            _drugRepository = drugRepository;
            _prescriptionRepository = prescriptionRepository;
        }
        public IPatientManager PatientManager => new PatientManager(_patientRepository);

        public IDrugManager DrugManager => new DrugManager(_drugRepository);

        public IPrescriptionManager PrescriptionManager => null;
    }
}
