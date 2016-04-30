using System.Linq;

namespace Service.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using com.pharmscription.BusinessLogic.Prescription;
    using com.pharmscription.Infrastructure.Dto;
    using com.pharmscription.Infrastructure.Exception;
    using com.pharmscription.Service.Routes;

    [RoutePrefix("")]
    public class PrescriptionController : Controller
    {
        private readonly IPrescriptionManager _prescriptionManager;

        public PrescriptionController(IPrescriptionManager manager)
        {
            _prescriptionManager = manager;
        }

        [Route(PrescriptionRoutes.GetPrescriptions)]
        [HttpGet]
        public async Task<ActionResult> GetPrescriptions(string patientid)
        {
            try
            {
                var prescriptions = await _prescriptionManager.Get(patientid);
                if (prescriptions.Any())
                {
                    return Json(prescriptions, JsonRequestBehavior.AllowGet);
                }
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
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

        [Route(PrescriptionRoutes.GetPrescriptionById)]
        public async Task<ActionResult> GetPrescriptionById(string patientid, string id)
        {
            try
            {
                return Json(await _prescriptionManager.Get(patientid, id), JsonRequestBehavior.AllowGet);
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

        [Route(PrescriptionRoutes.CreatePrescription)]
        [System.Web.Http.HttpPut]
        public async Task<ActionResult> CreatePrescription(string patientid, PrescriptionDto dto)
        {
            try
            {
                return Json(await _prescriptionManager.Add(patientid, dto));
            }
            catch (NotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            catch (ArgumentException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [Route(PrescriptionRoutes.GetCounterProposals)]
        public async Task<ActionResult> GetCounterProposals(string patientid, string prescriptionid)
        {
            try
            {
                var counterProposal = await _prescriptionManager.GetCounterProposals(patientid, prescriptionid);
                if (counterProposal != null && counterProposal.Any())
                {
                    return Json(await _prescriptionManager.GetCounterProposals(patientid, prescriptionid));
                }
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
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

        [Route(PrescriptionRoutes.CreateCounterProposal)]
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

        [Route(PrescriptionRoutes.GetDispenses)]
        public async Task<ActionResult> GetDispenses(string patientid, string prescriptionid)
        {
            try
            {
                var dispenses = await _prescriptionManager.GetDispense(patientid, prescriptionid);
                if (dispenses != null && dispenses.Any())
                {
                    return Json(dispenses);
                }
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
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

        [Route(PrescriptionRoutes.CreateDispense)]
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

        [Route(PrescriptionRoutes.GetDrugs)]
        public async Task<ActionResult> GetDrugs(string patientid, string prescriptionid)
        {
            try
            {
                var drugs = await _prescriptionManager.GetPrescriptionDrugs(patientid, prescriptionid);
                if (drugs != null && drugs.Any())
                {
                    return Json(drugs);
                }
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
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