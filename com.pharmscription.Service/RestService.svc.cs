
namespace com.pharmscription.Service
{
    using System;
    using System.Net;
    using System.ServiceModel.Web;
    using System.Threading.Tasks;

    using com.pharmscription.ApplicationFascade;
    using com.pharmscription.BusinessLogic.Drug;
    using com.pharmscription.BusinessLogic.Patient;
    using com.pharmscription.Infrastructure.Dto;
    using com.pharmscription.Infrastructure.Exception;

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
            PatientDto patient = null;
            try
            {
                patient = await _GetPatientManager().Find(ahv);
            }
            catch (InvalidAhvNumberException e)
            {
                SendError(HttpStatusCode.BadRequest, "AHV Number is not in a valid format");
            }
            catch (Exception e)
            {
                SendError(HttpStatusCode.BadRequest, e.Message);
            }
            if (patient == null)
            {
                SendError(HttpStatusCode.NotFound, "Patient with provided AHV number not found");
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

        private static void SendError(HttpStatusCode statusCode, string message)
        {
            OutgoingWebResponseContext response = WebOperationContext.Current?.OutgoingResponse;
            response.StatusCode = statusCode;
            response.StatusDescription = message;
        }

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
