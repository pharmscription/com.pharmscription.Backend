
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using com.pharmscription.BusinessLogic.Drug;
using com.pharmscription.Infrastructure.Exception;
using Service.Routes;

namespace Service.Controllers
{

    using log4net;

    [RoutePrefix("")]
    public class DrugController : Controller
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(DrugController));

        private readonly IDrugManager _drugManager;

        public DrugController(IDrugManager drugManager)
        {
            _drugManager = drugManager;
            _log.Debug("DrugController called");
        }

        [Route(DrugRoutes.GetDrugById)]
        public async Task<ActionResult> GetById(string id)
        {
            try
            {
                return Json(await _drugManager.GetById(id), JsonRequestBehavior.AllowGet);
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

        [Route(DrugRoutes.GetDrugsBySearchTerm)]
        public async Task<ActionResult> GetDrugsBySearchTerm(string keyword)
        {
            try
            {
                return Json(await _drugManager.Search(keyword), JsonRequestBehavior.AllowGet);
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

        [Route(DrugRoutes.GetDrugsCountBySearchTerm)]
        public async Task<ActionResult> GetDrugsCountBySearchTerm(string keyword)
        {
            try
            {
                return Json(await _drugManager.Count(keyword), JsonRequestBehavior.AllowGet);
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



        [Route(DrugRoutes.GetDrugsBySearchTermPaged)]
        public async Task<ActionResult> GetBySearchTermPaged(string keyword, string page, string amount)
        {
            try
            {
                return Json(await _drugManager.SearchPaged(keyword, page, amount), JsonRequestBehavior.AllowGet);
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
