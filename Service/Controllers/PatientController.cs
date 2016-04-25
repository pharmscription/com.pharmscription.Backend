using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using com.pharmscription.BusinessLogic.Patient;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.Exception;
using Service.Routes;

namespace Service.Controllers
{
    using System.Web.Mvc;

    [RoutePrefix("")]
    public class PatientController : Controller
    {
        private readonly IPatientManager _patientManager;

        public PatientController(IPatientManager patientManager)
        {
            _patientManager = patientManager;
        }

        [Route(PatientRoutes.GetPatientById)]
        public async Task<JsonResult> GetById(string id)
        {
            try
            {
                return Json(await _patientManager.GetById(id), JsonRequestBehavior.AllowGet);
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
        [Route(PatientRoutes.AddPatient)]
        [HttpPut]
        public async Task<JsonResult> Add(PatientDto patientDto)
        {
            try
            {
                return Json(await _patientManager.Add(patientDto), JsonRequestBehavior.AllowGet);
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

        [Route(PatientRoutes.GetPatientByAhvNumber)]
        public async Task<JsonResult> GetByAhv(string ahvNumber)
        {
            try
            {

                return Json(await _patientManager.Find(ahvNumber), JsonRequestBehavior.AllowGet);
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

        [Route(PatientRoutes.LookupPatientByAhvNumber)]
        public async Task<JsonResult> LookupByAhvNumber(string ahvNumber)
        {
            try
            {
                return Json(await _patientManager.Lookup(ahvNumber), JsonRequestBehavior.AllowGet);
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
