using System;
using System.Net;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Patient;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.Exception;
using com.pharmscription.Service.Routes;

namespace com.pharmscription.Service.Controllers
{
    using System.Web.Mvc;

    using com.pharmscription.Infrastructure.Constants;

    [RoutePrefix("")]
    public class PatientController : Controller
    {
        private readonly IPatientManager _patientManager;

        public PatientController(IPatientManager patientManager)
        {
            _patientManager = patientManager;
        }

        [PhaAuthorize[PhaAuthorize(Roles = PharmscriptionConstants.DOCTOR + "," + PharmscriptionConstants.DRUGIST + "," + PharmscriptionConstants.DRUGSTOREEMPLOYEE)]]
        [Route(PatientRoutes.GetPatientById)]
        public async Task<ActionResult> GetById(string id)
        {
            try
            {
                return Json(await _patientManager.GetById(id), JsonRequestBehavior.AllowGet);
            }
            catch (NotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            catch (ArgumentException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [PhaAuthorize(Roles = PharmscriptionConstants.DOCTOR)]
        [Route(PatientRoutes.AddPatient)]
        [HttpPut]
        public async Task<ActionResult> Add(PatientDto patientDto)
        {
            try
            {
                return Json(await _patientManager.Add(patientDto), JsonRequestBehavior.AllowGet);
            }
            catch (ArgumentException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [PhaAuthorize(Roles = PharmscriptionConstants.DOCTOR + "," + PharmscriptionConstants.DRUGIST + "," + PharmscriptionConstants.DRUGSTOREEMPLOYEE)]
        [Route(PatientRoutes.GetPatientByAhvNumber)]
        public async Task<ActionResult> GetByAhv(string ahv)
        {
            try
            {
                return Json(await _patientManager.Find(ahv), JsonRequestBehavior.AllowGet);
            }
            catch (NotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            catch (ArgumentException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [PhaAuthorize(Roles = PharmscriptionConstants.DOCTOR + "," + PharmscriptionConstants.DRUGIST + "," + PharmscriptionConstants.DRUGSTOREEMPLOYEE)]
        [Route(PatientRoutes.LookupPatientByAhvNumber)]
        public async Task<ActionResult> LookupByAhvNumber(string ahv)
        {
            try
            {
                var patient = await _patientManager.Lookup(ahv);
                if (patient != null)
                {
                    return Json(patient, JsonRequestBehavior.AllowGet);
                }
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            catch (ArgumentException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
    }
}
