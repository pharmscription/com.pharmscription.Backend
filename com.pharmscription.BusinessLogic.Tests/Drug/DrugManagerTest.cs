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
            Assert.AreEqual(4, drugs.Count);
        }

        [TestMethod]
        public async Task TestSearchReturnsEmptyListOnGarbage()
        {
            var drugs = await _drugManager.Search("jsdfkasdncknsacion");
            Assert.IsFalse(drugs.Any());
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
