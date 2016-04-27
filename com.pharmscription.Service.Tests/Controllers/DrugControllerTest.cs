using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using com.pharmscription.BusinessLogic.Drug;
using com.pharmscription.DataAccess.Repositories.Drug;
using com.pharmscription.DataAccess.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.Controllers;

namespace Service.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Web.Mvc;

    using com.pharmscription.Infrastructure.Dto;
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class DrugControllerTest
    {

        private DrugController _drugController;

        [TestInitialize]
        public void SetUp()
        {
            IPharmscriptionUnitOfWork puow = new PharmscriptionUnitOfWork();
            IDrugRepository repo = new DrugRepository(puow);
            IDrugManager drugManager = new DrugManager(repo);
            _drugController = new DrugController(drugManager);
        }


        [TestMethod]
        public async Task TestSearchPagedThrowsOnNegativeAmountPerPage()
        {
            var result = (HttpStatusCodeResult)await _drugController.GetBySearchTermPaged("Redimune", "2", "-1");
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestSearchPagedThrowsOnNegativePageNumber()
        {
            var result = (HttpStatusCodeResult)await _drugController.GetBySearchTermPaged("Redimune", "-1", "2");
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestCanDoSearchPaged()
        {
            var drugs = (List<DrugDto>)((JsonResult)await _drugController.GetBySearchTermPaged("Redimune", "0", "2")).Data;
            var drugsPageTwo = (List<DrugDto>)((JsonResult)await _drugController.GetBySearchTermPaged("Redimune", "1", "2")).Data;
            var drugsPageThree = (List<DrugDto>)((JsonResult)await _drugController.GetBySearchTermPaged("Redimune", "2", "2")).Data;
            Assert.AreEqual(2, drugs.Count);
            Assert.AreEqual(2, drugsPageTwo.Count);
            Assert.IsFalse(drugsPageThree.Any());
        }

        [TestMethod]
        public async Task TestGetByIdThrowsOnNull()
        {
            var result = (HttpStatusCodeResult)await _drugController.GetById(null);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetByIdThrowsOnEmpty()
        {
            var result = (HttpStatusCodeResult)await _drugController.GetById("");
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
