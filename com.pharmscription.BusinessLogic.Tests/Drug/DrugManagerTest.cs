using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Drug;
using com.pharmscription.DataAccess.Repositories.Drug;
using com.pharmscription.DataAccess.Tests.TestEnvironment;
using com.pharmscription.DataAccess.UnitOfWork;
using com.pharmscription.Infrastructure.Dto;
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
        public async Task TestDoesAddDrugFromDto()
        {
            var drugDto = new DrugDto
            {
                DrugDescription = "My Super Cool Test Drug"
            };
            await _drugManager.Add(drugDto);
            var drugFound = (await _drugManager.Search("Super Cool")).FirstOrDefault();
            Assert.IsNotNull(drugFound);
            Assert.AreEqual(drugDto.DrugDescription, drugFound.DrugDescription);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidArgumentException))]
        public async Task TestAddDtoThrowsOnNull()
        {
            await _drugManager.Add(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestEditThrowsOnEmpty()
        {

            await _drugManager.Edit(new DrugDto {Id = "    "});
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestEditThrowsOnNull()
        {
            await _drugManager.Edit(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestEditThrowsOnNotExistingDrug()
        {
            var drugDto = new DrugDto
            {
                Id = "a1b1a1b1-11df-12d3-abde-f365a7b889d0"
            };
            await _drugManager.Edit(drugDto);
        }

        [TestMethod]
        public async Task TestCanEditExistingDrugs()
        {
            var drugGuid = new Guid("a1b1a1b1-11df-12d3-abde-f365a7b889d0");
            var drugDto = new DrugDto
            {
                Id = drugGuid.ToString(),
                DrugDescription = "Unedited",
                IsValid = true,
                NarcoticCategory = "A"
            };
            await _drugManager.Add(drugDto);
            drugDto.DrugDescription = "Edited";
            drugDto.IsValid = false;
            drugDto.NarcoticCategory = "B";
            await _drugManager.Edit(drugDto);
            var drugEdited = await _drugManager.GetById(drugGuid.ToString());
            Assert.AreEqual("Edited", drugEdited.DrugDescription);
            Assert.IsFalse(drugEdited.IsValid);
            Assert.AreEqual("B", drugEdited.NarcoticCategory);
        }

        [TestMethod]
        public async Task TestCanGetById()
        {
            var searchGuid = new Guid("a1b1a1b1-11df-12d3-abde-f365a7b889d0");
            var drugDto = new DrugDto
            {
                DrugDescription = "Drug B",
                Id = searchGuid.ToString("N")
            };
            await _drugManager.Add(drugDto);
            var drugInserted = await _drugManager.GetById(searchGuid.ToString());
            Assert.IsNotNull(drugInserted);
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
        public async Task TestCanRemoveById()
        {
            var searchGuid = new Guid("a1b1a1b1-11df-12d3-abde-f365a7b889d0");
            var drugDto = new DrugDto
            {
                DrugDescription = "Drug B",
                Id = searchGuid.ToString()
            };
            await _drugManager.Add(drugDto);
            var drugInserted = await _drugManager.GetById(searchGuid.ToString());
            Assert.IsNotNull(drugInserted);
            await _drugManager.RemoveById(searchGuid.ToString());
            var drugAfterDeletion = await _drugManager.GetById(searchGuid.ToString());
            Assert.IsNull(drugAfterDeletion);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestRemoveByIdThrowsOnNull()
        {
            await _drugManager.RemoveById(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task TestRemoveByIdThrowsOnEmpty()
        {
            await _drugManager.RemoveById("");
        }


    }
}
