

namespace com.pharmscription.Service.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using BusinessLogic.DrugPrice;
    using Infrastructure.Exception;
    using Reporting;

    [RoutePrefix("")]
    public class ReportingController : Controller
    {
        private readonly IDrugPriceManager _drugPriceManager;
        private readonly Reporter _reporter;

        public ReportingController(IDrugPriceManager drugPriceManager, Reporter reporter)
        {
            _drugPriceManager = drugPriceManager;
            _reporter = reporter;
        }

        [HttpGet]
        [Route("reporting/seed")]
        public async Task<ActionResult> Seed()
        {
            try
            {
                await _drugPriceManager.SeedDataTables();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
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

        [HttpGet]
        [Route("reporting/report")]
        public async Task<ActionResult> Report()
        {
            try
            {
                await _reporter.WriteReports();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
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