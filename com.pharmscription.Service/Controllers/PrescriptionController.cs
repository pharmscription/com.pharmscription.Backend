namespace com.pharmscription.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using BusinessLogic.Prescription;
    using Infrastructure.Dto;
    using Infrastructure.Exception;
    using Routes;

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
        [HttpGet]
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
        [HttpPut]
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
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [Route(PrescriptionRoutes.UpdatePrescription)]
        [HttpPost]
        public async Task<ActionResult> UpdatePrescription(string patientid, string id, PrescriptionDto dto)
        {
            try
            {
                return Json(await _prescriptionManager.Update(patientid, id, dto));
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

        [Route(PrescriptionRoutes.GetCounterProposals)]
        [HttpGet]
        public async Task<ActionResult> GetCounterProposals(string patientid, string prescriptionid)
        {
            return await GetListOrException(_prescriptionManager.GetCounterProposals, patientid, prescriptionid);
        }

        [Route(PrescriptionRoutes.CreateCounterProposal)]
        [HttpPut]
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
        [HttpGet]
        public async Task<ActionResult> GetDispenses(string patientid, string prescriptionid)
        {
            return await GetListOrException(_prescriptionManager.GetDispenses, patientid, prescriptionid);
        }

        [Route(PrescriptionRoutes.CreateDispense)]
        [HttpPut]
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
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [Route(PrescriptionRoutes.EditDispense)]
        [HttpPost]
        public async Task<ActionResult> EditDispense(
            string patientid,
            string prescriptionid,
            string dispenseid,
            DispenseDto dispense)
        {
            try
            {
                return Json(await _prescriptionManager.ModifyDispense(patientid, prescriptionid, dispenseid, dispense));
            }
            catch (NotFoundException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
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

        [Route(PrescriptionRoutes.GetDrugs)]
        [HttpGet]
        public async Task<ActionResult> GetDrugs(string patientid, string prescriptionid)
        {
            return await GetListOrException(_prescriptionManager.GetPrescriptionDrugs, patientid, prescriptionid);
        }

        private async Task<ActionResult> GetListOrException<TDto>(Func<string, string, Task<ICollection<TDto>>> getEntites, string patientId, string prescriptionId) where TDto : class
        {
            try
            {
                var entites = await getEntites(patientId, prescriptionId);
                if (entites != null && entites.Any())
                {
                    return Json(entites);
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