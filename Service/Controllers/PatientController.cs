using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using com.pharmscription.BusinessLogic.Patient;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.Exception;
using Service.Routes;

namespace Service.Controllers
{
    [System.Web.Mvc.RoutePrefix("/patients")]
    public class PatientController : ApiController
    {
        private readonly IPatientManager _patientManager;

        public PatientController(IPatientManager patientManager)
        {
            _patientManager = patientManager;
        }

        [System.Web.Mvc.Route(PatientRoutes.GetPatientById)]
        public async Task<JsonResult<PatientDto>> GetById(string id)
        {
            try
            {
                return Json(await _patientManager.GetById(id));
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            catch (ArgumentException)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
        [System.Web.Mvc.Route(PatientRoutes.AddPatient)]
        [System.Web.Mvc.HttpPut]
        public async Task<JsonResult<PatientDto>> Add(PatientDto patientDto)
        {
            try
            {
                return Json(await _patientManager.Add(patientDto));
            }
            catch (ArgumentException e )
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [System.Web.Mvc.Route(PatientRoutes.GetPatientByAhvNumber)]
        public async Task<JsonResult<PatientDto>> GetByAhv(string ahvNumber)
        {
            try
            {

                return Json(await _patientManager.Find(ahvNumber));
            }

            catch (ArgumentException)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [System.Web.Mvc.Route(PatientRoutes.LookupPatientByAhvNumber)]
        public async Task<JsonResult<PatientDto>> LookupByAhvNumber(string ahvNumber)
        {
            try
            {
                return Json(await _patientManager.Lookup(ahvNumber));
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            catch (ArgumentException)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
    }
}
