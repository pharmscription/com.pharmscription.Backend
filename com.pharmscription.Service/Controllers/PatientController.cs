﻿using System;
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
        [System.Web.Mvc.Route(PatientRoutes.AddPatient)]
        [HttpPut]
        public async Task<ActionResult> Add(PatientDto patientDto)
        {
            try
            {
                return Json(await _patientManager.Add(patientDto), JsonRequestBehavior.AllowGet);
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

        [System.Web.Mvc.Route(PatientRoutes.GetPatientByAhvNumber)]
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

        [System.Web.Mvc.Route(PatientRoutes.LookupPatientByAhvNumber)]
        public async Task<ActionResult> LookupByAhvNumber(string ahv)
        {
            try
            {
                return Json(await _patientManager.Lookup(ahv), JsonRequestBehavior.AllowGet);
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