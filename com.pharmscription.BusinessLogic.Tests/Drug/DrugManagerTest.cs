using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Drug;
using com.pharmscription.DataAccess.Repositories.Drug;
using com.pharmscription.DataAccess.Tests.TestEnvironment;
using com.pharmscription.DataAccess.UnitOfWork;
using com.pharmscription.Infrastructure.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.BusinessLogic.Tests.Drug
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DrugManagerTest
    {
        private IDrugManager _drugManager;

        [TestInitialize]
        public void SetUp()
        {
            var mockSet = TestEnvironmentHelper.GetMockedAsyncProviderDbSet(new List<DataAccess.Entities.DrugEntity.Drug>());

            var mockPuow = TestEnvironmentHelper.GetMockedDataContext();
            mockPuow.Setup(m => m.Drugs).Returns(mockSet.Object);
            mockPuow.Setup(m => m.CreateSet<DataAccess.Entities.DrugEntity.Drug>()).Returns(mockSet.Object);
            IPharmscriptionUnitOfWork puow = mockPuow.Object;
            IDrugRepository repository = new DrugRepository(puow);
            _drugManager = new DrugManager(repository);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestSearchThrowsOnNull()
        {
            await _drugManager.Search(null);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidArgumentException))]
        public async Task TestSearchThrowsOnEmpty()
        {
            await _drugManager.Search("");
        }

        [TestMethod]
        public async Task TestCanDoSearch()
        {
            var drugs = await _drugManager.Search("Redimune");
            Assert.AreEqual(4, drugs);
        }

        [TestMethod]
        public async Task TestSearchReturnsEmptyListOnGarbage()
        {
            var drugs = await _drugManager.Search("jsdfkasdncknsacion");
            Assert.AreEqual(drugs.Count, 0);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestSearchPagedThrowsOnNegativeAmountPerPage()
        {
            await _drugManager.SearchPaged("Redimune", "2", "-1");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestSearchPagedThrowsOnNegativePageNumber()
        {
            await _drugManager.SearchPaged("Redimune", "-1", "2");
        }

        [TestMethod]
        public async Task TestCanDoSearchPaged()
        {
            var drugs = await _drugManager.SearchPaged("Redimune", "0", "2");
            var drugsPageTwo = await _drugManager.SearchPaged("Redimune", "1", "2");
            var drugsPageThree = await _drugManager.SearchPaged("Redimune", "2", "2");
            Assert.AreEqual(2, drugs.Count);
            Assert.AreEqual(2, drugsPageTwo.Count);
            Assert.IsFalse(drugsPageThree.Any());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetByIdThrowsOnNull()
        {
            await _drugManager.GetById(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestGetByIdThrowsOnEmpty()
        {
            await _drugManager.GetById("");
        }
    }
}
