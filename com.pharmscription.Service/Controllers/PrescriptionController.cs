namespace Service.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Mvc;

    using com.pharmscription.BusinessLogic.Prescription;
    using com.pharmscription.Infrastructure.Dto;

    using Routes;
    using com.pharmscription.Infrastructure.Exception;

    [System.Web.Mvc.RoutePrefix("")]
    public class PrescriptionController : Controller
    {
        private readonly IPrescriptionManager _prescriptionManager;

        public PrescriptionController(IPrescriptionManager manager)
        {
            _prescriptionManager = manager;
        }
        [System.Web.Mvc.Route(PrescriptionRoutes.GetPrescriptions)]
        public async Task<ActionResult> GetPrescriptions(string patientid)
        {
            try
            {
                return Json(await _prescriptionManager.Get(patientid));
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
        [System.Web.Mvc.Route(PrescriptionRoutes.GetPrescriptionById)]
        public async Task<ActionResult> GetPrescriptionById(string patientid, string prescriptionid)
        {
            try
            {
                return Json(await _prescriptionManager.Get(patientid, prescriptionid));
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

        [System.Web.Mvc.Route(PrescriptionRoutes.CreatePrescription)]
        [System.Web.Http.HttpPut]
        public async Task<ActionResult> CreatePrescription(string patientid, PrescriptionDto dto)
        {
            try
            {
                return Json(await _prescriptionManager.Add(patientid, dto));
            }
            catch (NotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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

        [System.Web.Mvc.Route(PrescriptionRoutes.GetCounterProposals)]
        public async Task<ActionResult> GetCounterProposals(string patientid, string prescriptionid)
        {
            try
            {
                return Json(await _prescriptionManager.GetCounterProposal(patientid, prescriptionid));
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

        [System.Web.Mvc.Route(PrescriptionRoutes.CreateCounterProposal)]
        [System.Web.Http.HttpPut]
        public async Task<ActionResult> CreateCounterProposal(
            string patientid,
            string prescriptionid,
            CounterProposalDto dto)
        {
            try
            {
                return Json(await _prescriptionManager.AddCounterProposal(patientid, prescriptionid, dto));
            }
            catch (NotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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

        [System.Web.Mvc.Route(PrescriptionRoutes.GetDispenses)]
        public async Task<ActionResult> GetDispenses(string patientid, string prescriptionid)
        {
            try
            {
                return Json(await _prescriptionManager.GetDispense(patientid, prescriptionid));
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

        [System.Web.Mvc.Route(PrescriptionRoutes.CreateDispense)]
        [System.Web.Http.HttpPut]
        public async Task<ActionResult> CreateDispense(
            string patientid,
            string prescriptionid,
            DispenseDto dto)
        {
            try
            {
                return Json(await _prescriptionManager.AddDispense(patientid, prescriptionid, dto));
            }
            catch (NotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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

        [System.Web.Mvc.Route(PrescriptionRoutes.GetDrugs)]
        public async Task<ActionResult> GetDrugs(string patientid, string prescriptionid)
        {
            try
            {
                return Json(await _prescriptionManager.GetPrescriptionDrugs(patientid, prescriptionid));
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
    }
}