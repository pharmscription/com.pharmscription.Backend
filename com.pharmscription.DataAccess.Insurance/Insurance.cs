using System.Threading.Tasks;
using com.pharmscription.Infrastructure.ExternalDto.InsuranceDto;

namespace com.pharmscription.DataAccess.Insurance
{
    public class Insurance : IInsurance
    {
        private readonly IPatientStore _patientStore;

        public Insurance(IPatientStore patientStore)
        {
            _patientStore = patientStore;
        }

        public async Task<InsurancePatient> FindPatient(string ahvNumber)
        {
            return await Task.Run(() => (_patientStore.Patients.Find(p => p.AhvNumber == ahvNumber)));
        }

        public static IInsurance RealInsurance => new Insurance(new PatientStore());
    }

    
}
