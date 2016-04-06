using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Drug;
using com.pharmscription.Infrastructure.Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.BusinessLogic.Tests.Drug
{
    [TestClass]
    public class DrugManagerTest
    {
        private IDrugManager _drugManager;

        [TestInitialize]
        public void SetUp()
        {
            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task TestSearchThrowsOnNull()
        {
            var drugs = await _drugManager.Search(null);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public async Task TestSearchThrowsOnEmpty()
        {
            var drugs = await _drugManager.Search("");
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

        /*[TestMethod]
        public async Task TestDoesAddDrugFromDto()
        {
            var 
        }

        public Task<DrugDto> Add(DrugDto drug)
        {
            throw new System.NotImplementedException();
        }

        public Task<DrugDto> Edit(DrugDto drug)
        {
            throw new System.NotImplementedException();
        }

        public Task<DrugDto> GetById(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<DrugDto> RemoveById(string id)
        {
            throw new System.NotImplementedException();
        }


        [TestMethod]
        public void TestMethod1()
        {
        }*/
    }
}
