﻿namespace Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Results;
    using System.Web.Mvc;

    using com.pharmscription.BusinessLogic.Prescription;
    using com.pharmscription.Infrastructure.Dto;

    using Service.Routes;
    using com.pharmscription.Infrastructure.Exception;
    public class prescriptionsController : ApiController
    {
        private readonly IPrescriptionManager _prescriptionManager;

        public prescriptionsController(IPrescriptionManager manager)
        {
            _prescriptionManager = manager;
        }
        [System.Web.Mvc.Route(PrescriptionRoutes.GetPrescriptions)]
        public async Task<JsonResult<List<PrescriptionDto>>> GetPrescriptions(string patientid)
        {
            try
            {
                return Json(await _prescriptionManager.Get(patientid));
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
        [System.Web.Mvc.Route(PrescriptionRoutes.GetPrescriptionById)]
        public async Task<JsonResult<PrescriptionDto>> GetPrescriptionById(string patientid, string prescriptionid)
        {
            try
            {
                return Json(await _prescriptionManager.Get(patientid, prescriptionid));
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

        [System.Web.Mvc.Route(PrescriptionRoutes.CreatePrescription)]
        [System.Web.Http.HttpPut]
        public async Task<JsonResult<PrescriptionDto>> CreatePrescription(string patientid, PrescriptionDto dto)
        {
            try
            {
                return Json(await _prescriptionManager.Add(patientid, dto));
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

        [System.Web.Mvc.Route(PrescriptionRoutes.GetCounterProposals)]
        public async Task<JsonResult<List<CounterProposalDto>>> GetCounterProposals(string patientid, string prescriptionid)
        {
            try
            {
                return Json(await _prescriptionManager.GetCounterProposal(patientid, prescriptionid));
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

        [System.Web.Mvc.Route(PrescriptionRoutes.CreateCounterProposal)]
        [System.Web.Http.HttpPut]
        public async Task<JsonResult<CounterProposalDto>> CreateCounterProposal(
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

        [System.Web.Mvc.Route(PrescriptionRoutes.GetDispenses)]
        public async Task<JsonResult<List<DispenseDto>>> GetDispenses(string patientid, string prescriptionid)
        {
            try
            {
                return Json(await _prescriptionManager.GetDispense(patientid, prescriptionid));
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

        [System.Web.Mvc.Route(PrescriptionRoutes.CreateDispense)]
        [System.Web.Http.HttpPut]
        public async Task<JsonResult<DispenseDto>> CreateDispense(
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

        [System.Web.Mvc.Route(PrescriptionRoutes.GetDrugs)]
        public async Task<JsonResult<DrugItemDto[]>> GetDrugs(string patientid, string prescriptionid)
        {
            try
            {
                return Json((await _prescriptionManager.GetPrescriptionDrugs(patientid, prescriptionid)).ToArray());
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