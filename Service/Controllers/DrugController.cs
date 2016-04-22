
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using com.pharmscription.BusinessLogic.Drug;
using com.pharmscription.Infrastructure.Exception;
using Service.Routes;

namespace Service.Controllers
{
    public class DrugController : Controller
    {
        private readonly IDrugManager _drugManager;

        public DrugController(IDrugManager drugManager)
        {
            _drugManager = drugManager;
        }

        [System.Web.Mvc.Route(DrugRoutes.GetDrugById)]
        public async Task<ActionResult> GetById(string id)
        {
            try
            {
                return Json(await _drugManager.GetById(id));
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

        [System.Web.Mvc.Route(DrugRoutes.GetDrugsCountBySearchTerm)]
        public async Task<ActionResult> GetCountBySearchTerm(string keyword)
        {
            try
            {
                return Json(await _drugManager.Search(keyword));
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

        [System.Web.Mvc.Route(DrugRoutes.GetDrugsBySearchTermPaged)]
        public async Task<ActionResult> GetBySearchTermPaged(string keyword, string page, string amount)
        {
            try
            {
                return Json((await _drugManager.SearchPaged(keyword, page, amount)).ToArray());
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
