
using com.pharmscription.ApplicationFascade;
using com.pharmscription.Security.SessionStore;

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

        private readonly IPharmscriptionApplication _application;

        public RestService()
        {
            _application = new PharmscriptionApplication();
        }

        #region patient
        public async Task<PatientDto> GetPatient(string id)
        {
            try
            {
                return await _application.ManagerFactory("id").PatientManager.GetById(id);
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
            return await _application.ManagerFactory("id").PatientManager.Add(dto);
        }

        
        public async Task<PatientDto> GetPatientByAhv(string ahv)
       {
            try
            {
                PatientDto dto = await _application.ManagerFactory("id").PatientManager.Find(ahv); ;
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
                return await _application.ManagerFactory("id").PatientManager.Lookup(ahv);
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
                return await _application.ManagerFactory("sessionid").DrugManager.GetById(id);
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
                return (await _application.ManagerFactory("sessionid").DrugManager.Search(keyword)).ToArray();
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
