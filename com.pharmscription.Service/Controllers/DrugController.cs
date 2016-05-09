
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using com.pharmscription.BusinessLogic.Drug;
using com.pharmscription.Infrastructure.Exception;

namespace com.pharmscription.Service.Controllers
{
    using com.pharmscription.Infrastructure.Constants;

    using Routes;

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

        [PhaAuthorize]
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

        [PhaAuthorize]
        [Route(DrugRoutes.GetDrugsCountBySearchTerm)]
        public async Task<ActionResult> GetDrugsCountBySearchTerm(string keyword)
        {
            try
            {
                return Json(await _drugManager.Count(keyword), JsonRequestBehavior.AllowGet);
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


        [PhaAuthorize]
        [Route(DrugRoutes.GetDrugsBySearchTermPaged)]
        public async Task<ActionResult> GetBySearchTermPaged(string keyword, string page, string amount)
        {
            try
            {
                var drugs = await _drugManager.SearchPaged(keyword, page, amount);
                if (drugs.Any())
                {
                    return Json(drugs, JsonRequestBehavior.AllowGet);
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
