using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.Infrastructure.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.DataAccess.SwissMedic.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
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
        [ExpectedException(typeof(InvalidArgumentException))]
        public async Task ThrowsOnNull()
        {
            await _swissMedic.SearchDrug(null);
        }

        [TestMethod]
        [ExpectedException(typeof (InvalidArgumentException))]
        public async Task ThrowOnEmpty()
        {
            await _swissMedic.SearchDrug("");
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
