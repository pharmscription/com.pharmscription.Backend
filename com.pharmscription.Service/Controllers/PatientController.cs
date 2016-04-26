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

    [System.Web.Mvc.RoutePrefix("")]
    public class PatientController : Controller
    {
        private readonly IPatientManager _patientManager;

        public PatientController(IPatientManager patientManager)
        {
            _patientManager = patientManager;
        }

        [System.Web.Mvc.Route(PatientRoutes.GetPatientById)]
        public async Task<JsonResult> GetById(string id)
        {
            try
            {
                return Json(await _patientManager.GetById(id), JsonRequestBehavior.AllowGet);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NoContent);
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

        [System.Web.Mvc.Route(PatientRoutes.GetPatientByAhvNumber)]
        public async Task<JsonResult> GetByAhv(string ahv)
        {
            try
            {
                return Json(await _patientManager.Find(ahv), JsonRequestBehavior.AllowGet);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NoContent);
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
        public async Task<JsonResult> LookupByAhvNumber(string ahv)
        {
            try
            {
                return Json(await _patientManager.Lookup(ahv), JsonRequestBehavior.AllowGet);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NoContent);
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
