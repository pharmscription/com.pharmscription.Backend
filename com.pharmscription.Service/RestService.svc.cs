
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
        private readonly ManagerFactory _managerFactory;
        private IPatientManager _patientManager;
        private IDrugManager _drugManager;

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
            _managerFactory = new ManagerFactory();
        }

        #region patient
        public async Task<PatientDto> GetPatient(string id)
        {
            return await _GetPatientManager().GetById(id);
        }

        public async Task<PatientDto> CreatePatient(PatientDto dto)
        {
            return await _GetPatientManager().Add(dto);
        }

       public async Task<PatientDto> GetPatientByAhv(string ahv)
        {
            var patient = await _GetPatientManager().Find(ahv);
            if (patient == null)
            {
                return new PatientDto();
            }

            return patient;
        }

        public async Task<PatientDto> LookupPatient(string ahv)
        {
            return await _GetPatientManager().Lookup(ahv);
        }
        #endregion

        #region drugs
        public async Task<DrugDto> GetDrug(string id)
        {
            return await _GetDrugManager().GetById(id);
        }

        public async Task<DrugDto[]> SearchDrugs(string keyword)
        {
            return (await _GetDrugManager().Search(keyword)).ToArray();
        }
        #endregion

        private IPatientManager _GetPatientManager()
        {
            if (_patientManager == null)
            {
                _patientManager = _managerFactory.PatientManager;
            }
            return _patientManager;
        }

        private IDrugManager _GetDrugManager()
        {
            if (_drugManager == null)
            {
                _drugManager = _managerFactory.DrugManager;
            }
            return _drugManager;
        }
        
    }
}
