
namespace com.pharmscription.Service
{
    using System;
    using System.Net;
    using System.ServiceModel;
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
            try
            {
                return await _GetPatientManager().GetById(id);
            }
            catch (NotFoundException e)
            {
                throw new WebFaultException<ErrorMessage>(new ErrorMessage(e.Message), HttpStatusCode.NotFound);
            }
            catch (Exception e)
            {
                throw new WebFaultException<ErrorMessage>(new ErrorMessage(e.Message), HttpStatusCode.BadRequest);
            }
        }

        public async Task<PatientDto> CreatePatient(PatientDto dto)
        {
            try
            {
                return await _GetPatientManager().Add(dto);
            }
            catch (Exception e)
            {
                throw new WebFaultException<ErrorMessage>(new ErrorMessage(e.Message), HttpStatusCode.BadRequest);
            }
        }

        public async Task<PatientDto> GetPatientByAhv(string ahv)
       {
            try
            {
                PatientDto dto = await _GetPatientManager().Find(ahv);
                if (dto == null)
                {
                    throw new WebFaultException<ErrorMessage>(new ErrorMessage("Patient not found"), HttpStatusCode.NotFound);
                }
                return dto;
            }
            catch (Exception e)
            {
                throw new WebFaultException<ErrorMessage>(new ErrorMessage(e.Message), HttpStatusCode.BadRequest);
            }
        }

        public async Task<PatientDto> LookupPatient(string ahv)
        {
            try
            {
                return await _GetPatientManager().Lookup(ahv);
            }
            catch (NotFoundException e)
            {
                throw new WebFaultException<ErrorMessage>(new ErrorMessage(e.Message), HttpStatusCode.NotFound);
            }
            catch (Exception e)
            {
                throw new WebFaultException<ErrorMessage>(new ErrorMessage(e.Message), HttpStatusCode.BadRequest);
            }
        }
        #endregion

        #region drugs
        public async Task<DrugDto> GetDrug(string id)
        {
            try
            {
                return await _GetDrugManager().GetById(id);
            }
            catch (NotFoundException e)
            {
                throw new WebFaultException<ErrorMessage>(new ErrorMessage(e.Message), HttpStatusCode.NotFound);
            }
            catch (Exception e)
            {
                throw new WebFaultException<ErrorMessage>(new ErrorMessage(e.Message), HttpStatusCode.BadRequest);
            }
        }

        public async Task<DrugDto[]> SearchDrugs(string keyword)
        {
            try
            {
                return (await _GetDrugManager().Search(keyword)).ToArray();
            }
            catch (NotFoundException e)
            {
                throw new WebFaultException<ErrorMessage>(new ErrorMessage(e.Message), HttpStatusCode.NotFound);
            }
            catch (Exception e)
            {
                throw new WebFaultException<ErrorMessage>(new ErrorMessage(e.Message), HttpStatusCode.BadRequest);
            }
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
