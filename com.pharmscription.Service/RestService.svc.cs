
namespace com.pharmscription.Service
{
    using System;
    using System.Net;
    using System.ServiceModel.Web;
    using System.Threading.Tasks;
    using BusinessLogic.Drug;
    using BusinessLogic.Patient;
    using Infrastructure.Dto;
    using Infrastructure.Exception;

    public class RestService : IRestService
    {
        private readonly IPatientManager _patientManager;
        private readonly IDrugManager _drugManager;


        public RestService(IPatientManager patientManager, IDrugManager drugManager)
        {
            _patientManager = patientManager;
            _drugManager = drugManager;
        }


        #region patient
        public async Task<PatientDto> GetPatient(string id)
        {
            try
            {
                return await _patientManager.GetById(id);
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
                return await _patientManager.Add(dto);
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
                PatientDto dto = await _patientManager.Find(ahv);
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
                return await _patientManager.Lookup(ahv);
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
                return await _drugManager.GetById(id);
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
                return (await _drugManager.Search(keyword)).ToArray();
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
    }
}
