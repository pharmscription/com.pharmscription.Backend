using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using com.pharmscription.BusinessLogic.Patient;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.Exception;

namespace Service.Controllers
{
    public class patientsController : Controller
    {
        private readonly IPatientManager _patientManager;

        public patientsController(IPatientManager patientManager)
        {
            _patientManager = patientManager;
        }
/*        // GET: patients
        public ActionResult Index()
        {
            return null;
        }*/

        // GET: patients/Details/5
        public async Task<ActionResult> Index(string id)
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

        [System.Web.Mvc.HttpPut]
        public async Task<ActionResult> Index(PatientDto patientDto)
        {
            try
            {
                return Json(await _patientManager.Add(patientDto));
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

        [System.Web.Mvc.Route("patients/ahv-number/{ahv}")]
        public async Task<ActionResult> GetByAhv(string ahvNumber)
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

        [System.Web.Mvc.Route("patients/lookup/{ahv}")]
        public async Task<ActionResult> LookupByAhvNumber(string ahvNumber)
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
