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
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestSearchPagedThrowsOnNegativeAmountPerPage()
        {
            await _drugController.GetBySearchTermPaged("Redimune", "2", "-1");
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestSearchPagedThrowsOnNegativePageNumber()
        {
            await _drugController.GetBySearchTermPaged("Redimune", "-1", "2");
        }

        [TestMethod]
        public async Task TestCanDoSearchPaged()
        {
            var drugs = (List<DrugDto>)(await _drugController.GetBySearchTermPaged("Redimune", "0", "2")).Data;
            var drugsPageTwo = (List<DrugDto>)(await _drugController.GetBySearchTermPaged("Redimune", "1", "2")).Data;
            var drugsPageThree = (List<DrugDto>)(await _drugController.GetBySearchTermPaged("Redimune", "2", "2")).Data;
            Assert.AreEqual(2, drugs.Count);
            Assert.AreEqual(2, drugsPageTwo.Count);
            Assert.IsFalse(drugsPageThree.Any());
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetByIdThrowsOnNull()
        {
            await _drugController.GetById(null);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetByIdThrowsOnEmpty()
        {
            await _drugController.GetById("");
        }

/*        [TestMethod]
        public async Task TestCanDoSearchPagedCount()
        {
            var drugs = (await _drugController.GetCountBySearchTerm("Redimune")).Content;
            Assert.AreEqual(2, drugs.Count);
            Assert.AreEqual(2, drugsPageTwo.Count);
            Assert.IsFalse(drugsPageThree.Any());
        }*/
    }
}
