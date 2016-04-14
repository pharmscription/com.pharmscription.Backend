
using com.pharmscription.Security.SessionStore;

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
        private readonly PharmscriptionApplication _application;
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
            _application = new PharmscriptionApplication();
        }

        #region patient
        public async Task<PatientDto> GetPatient(string id)
        {
            return await _application.ManagerFactory("id").PatientManager.GetById(id);
        }

        public async Task<PatientDto> CreatePatient(PatientDto dto)
        {
            return await _application.ManagerFactory("id").PatientManager.Add(dto);
        }

        public async Task<PatientDto> GetPatientByAhv(string ahv)
        {
            var patient = await _application.ManagerFactory("id").PatientManager.Find(ahv);
            if (patient == null)
            {
                return new PatientDto();
            }

            return patient;
        }

        public async Task<PatientDto> LookupPatient(string ahv)
        {
            return await _application.ManagerFactory("id").PatientManager.Lookup(ahv);
        }
        #endregion

        #region drugs
        public async Task<DrugDto> GetDrug(string id)
        {
            return await _application.ManagerFactory("sessionid").DrugManager.GetById(id);
        }

        public async Task<DrugDto[]> SearchDrugs(string keyword)
        {
            return (await _application.ManagerFactory("sessionid").DrugManager.Search(keyword)).ToArray();
        }

        #endregion

        #region login

        public async Task<SessionDto> Login(LoginDto dto)
        {
            Session session = await _application.Authenticate(dto.Username, dto.Password);

            return new SessionDto
            {
                SessionId = session.Token
            };
        }


        #endregion
    }
}
