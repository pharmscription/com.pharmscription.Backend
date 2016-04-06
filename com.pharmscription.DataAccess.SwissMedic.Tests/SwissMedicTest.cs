using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.DataAccess.SwissMedic.Tests
{
    [TestClass]
    public class SwissMedicTest
    {
        private ISwissMedic _swissMedic;

        [TestInitialize]
        public void SetUp()
        {
            _swissMedic = new SwissMedicMock();
        }

        [TestMethod]
        public async Task TestCanReadAll()
        {
            var allDrugs = await _swissMedic.GetAll();
            Assert.AreEqual(12534, allDrugs.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ThrowsOnNull()
        {
            var drug = await _swissMedic.SearchDrug(null);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public async Task ThrowOnEmpty()
        {
            var drug = await _swissMedic.SearchDrug("");
        }

        [TestMethod]
        public async Task TestGetsItemsSearched()
        {
            var drugs = await _swissMedic.SearchDrug("Redimune");
            Assert.AreEqual(4, drugs.Count);
        }

        [TestMethod]
        public async Task TestReturnEmptyListInGarbage()
        {
            var drugs = await _swissMedic.SearchDrug("ksjdfkasflkjasfksan");
            Assert.IsFalse(drugs.Any());
        }
    }
}
