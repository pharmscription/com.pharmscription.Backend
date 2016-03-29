using com.pharmscription.Infrastructure.Insurance;

namespace com.pharmscription.DataAccess.Insurance
{
    public class Insurance : IInsurance
    {
        private readonly IPatientStore _patientStore;

        public Insurance(IPatientStore patientStore)
        {
            this._patientStore = patientStore;
        }

        public InsurancePatient FindPatient(string ahvNumber)
        {
            return _patientStore.Patients.Find(p => p.AhvNumber == ahvNumber);
        }

        public static IInsurance ZurichInsurance => new Insurance(new PatientStore());
    }

    
}
