﻿using System.Collections.Generic;
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
        private IDrugRepository _drugRepository;

        [TestInitialize]
        public void SetUp()
        {
            var mockSet = TestEnvironmentHelper.GetMockedAsyncProviderDbSet(new List<DataAccess.Entities.DrugEntity.Drug>());

            var mockPuow = TestEnvironmentHelper.GetMockedDataContext();
            mockPuow.Setup(m => m.Drugs).Returns(mockSet.Object);
            mockPuow.Setup(m => m.CreateSet<DataAccess.Entities.DrugEntity.Drug>()).Returns(mockSet.Object);
            IPharmscriptionUnitOfWork puow = mockPuow.Object;
            _drugRepository = new DrugRepository(puow);
            _drugManager = new DrugManager(_drugRepository);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestSearchPagedThrowsOnNegativeAmountPerPage()
        {
            await _drugManager.SearchPaged("Redimune", "2", "-1");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestSearchPagedThrowsOnEmptySearchTerm()
        {
            await _drugManager.SearchPaged("     ", "2", "3");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestSearchPagedThrowsOnPageNumberNotANumber()
        {
            await _drugManager.SearchPaged("Aspirin", "Number", "3");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestSearchPagedThrowsOnAmountNotANumber()
        {
            await _drugManager.SearchPaged("Aspirin", "2", "Number");
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

        [TestMethod]
        public async Task TestCountDoesLazyLoading()
        {
            foreach (var drug in _drugRepository.GetAll().ToList())
            {
                _drugRepository.Remove(drug);
            }
            await _drugRepository.UnitOfWork.CommitAsync();
            var drugCount = await _drugManager.Count("Redimune");
            Assert.AreEqual(4, drugCount);
        }
    }
}
