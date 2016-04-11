
namespace com.pharmscription.Service
{
    using System;
    using System.Threading.Tasks;

    using com.pharmscription.ApplicationFascade;
    using com.pharmscription.BusinessLogic.Drug;
    using com.pharmscription.BusinessLogic.Patient;
    using com.pharmscription.Infrastructure.Dto;

    public class RestService : IRestService
    {
        private readonly IPatientManager _patientManager;

        private readonly IDrugManager _drugManager;

        public RestService(IPatientManager patientManager)
        {
            _patientManager = patientManager;
        }

        public RestService(IDrugManager drugManager)
        {
            _drugManager = drugManager;
        }

        public RestService()
        {
            _patientManager = new ManagerFactory().PatientManager;
            _drugManager = new ManagerFactory().DrugManager;
        }

        #region patient
        public async Task<PatientDto> GetPatient(string id)
        {
            return await _patientManager.GetById(id);
        }

        public async Task<PatientDto> CreatePatient(PatientDto dto)
        {
            return await _patientManager.Add(dto);
        }

        public async Task<PatientDto> ModifyPatient(string id, PatientDto newPatientDto)
        {
            throw new NotImplementedException();
        }

        public async Task<AddressDto> GetAddress(string patientId)
        {
            throw new NotImplementedException();
        }

        public async Task<PatientDto> GetPatientByAhv(string ahv)
        {
            var patient = await _patientManager.Find(ahv);
            if (patient == null)
            {
                return new PatientDto();
            }

            return patient;
        }

        public async Task<PatientDto> LookupPatient(string ahv)
        {
            return await _patientManager.Lookup(ahv);
        }

        public async Task<PatientDto> DeletePatient(string id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region drugs
        public async Task<DrugDto> GetDrug(string id)
        {
            return await _drugManager.GetById(id);
        }

        public async Task<DrugDto[]> SearchDrugs(string keyword)
        {
            return (await _drugManager.Search(keyword)).ToArray();
        }

        public Task<double> GetDrugPrice(string id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
