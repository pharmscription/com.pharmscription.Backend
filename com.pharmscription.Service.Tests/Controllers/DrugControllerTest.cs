﻿using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Drug;
using com.pharmscription.DataAccess.Repositories.Drug;
using com.pharmscription.DataAccess.UnitOfWork;
using com.pharmscription.Infrastructure.EntityHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.Service.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Web.Mvc;

    using Infrastructure.Dto;
    using Service.Controllers;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class DrugControllerTest
    {

        private DrugController _drugController;
        private IDrugManager _drugManager;

        [ClassInitialize]
        public static void SetUpClass(TestContext testContext)
        {
            IPharmscriptionUnitOfWork puow = new PharmscriptionUnitOfWork();
            puow.ExecuteCommand("Delete From DrugPrices");
            puow.ExecuteCommand("Delete From DrugItems");
            puow.ExecuteCommand("Delete From Drugs");
        }

        [TestInitialize]
        public void SetUp()
        {
            IPharmscriptionUnitOfWork puow = new PharmscriptionUnitOfWork();
            IDrugRepository repo = new DrugRepository(puow);
            _drugManager = new DrugManager(repo);
            _drugController = new DrugController(_drugManager);
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
            var drugsPageThree = (HttpStatusCodeResult)await _drugController.GetBySearchTermPaged("Redimune", "2", "2");
            Assert.AreEqual(2, drugs.Count);
            Assert.AreEqual(2, drugsPageTwo.Count);
            Assert.AreEqual((int)HttpStatusCode.NoContent, drugsPageThree.StatusCode);
        }

        [TestMethod]
        public async Task TestSearchPagedReturnsNoContentListOnNotFound()
        {
            var result = (HttpStatusCodeResult) await _drugController.GetBySearchTermPaged("asdfiasdölkfklsadfkjasdfklasdf", "0", "2");
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
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

        [TestMethod]
        public async Task TestByIdReturnsNoContentOnNotFound()
        {
            var result =
                (HttpStatusCodeResult) await _drugController.GetById(IdentityGenerator.NewSequentialGuid().ToString());
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [TestMethod]
        public async Task TestById()
        {
            var testDrug = (await _drugManager.SearchPaged("Redimune", "1", "1")).FirstOrDefault();
            Assert.IsNotNull(testDrug);
            var drugFoundById = (DrugDto)((JsonResult)await _drugController.GetById(testDrug.Id)).Data;
            Assert.AreEqual(testDrug.DrugDescription, drugFoundById.DrugDescription);
        }
        [TestMethod]
        public async Task TestCountReturnsBadRequestOnNull()
        {
            var result = (HttpStatusCodeResult)await _drugController.GetDrugsCountBySearchTerm(null);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestCountReturnsBadRequestOnEmpty()
        {
            var result = (HttpStatusCodeResult)await _drugController.GetDrugsCountBySearchTerm("     ");
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestCanDoCount()
        {
            var drugCount = (int)((JsonResult)await _drugController.GetDrugsCountBySearchTerm("Redimune")).Data;
            Assert.AreEqual(4, drugCount);
        }

        [TestMethod]
        public async Task TestCountReturnsEmptyListOnNotFound()
        {
            var result = (int)((JsonResult)await _drugController.GetDrugsCountBySearchTerm("asdfiasdölkfklsadfkjasdfklasdf")).Data;
            Assert.AreEqual(0, result);
        }
    }
}
