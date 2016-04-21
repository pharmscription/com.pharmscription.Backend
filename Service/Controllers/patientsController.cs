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
            catch (NotFoundException e)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            catch (ArgumentException e)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        // GET: patients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: patients/Create
        [System.Web.Mvc.HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: patients/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: patients/Edit/5
        [System.Web.Mvc.HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: patients/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: patients/Delete/5
        [System.Web.Mvc.HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
