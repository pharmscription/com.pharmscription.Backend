
namespace com.pharmscription.Reporting.Tests
{
    using System.IO;
    using System.Threading.Tasks;
    using BusinessLogic.DrugPrice;
    using DataAccess.Repositories.Drug;
    using DataAccess.Repositories.DrugPrice;
    using DataAccess.Repositories.DrugStore;
    using DataAccess.Tests.TestEnvironment;
    using DataAccess.UnitOfWork;
    using Infrastructure.Exception;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PdfReportWriterTests
    {
        private PdfReportWriter _pdfReportWriter;

        [TestInitialize]
        public void SetUp()
        {
            var puow = new PharmscriptionUnitOfWork();
            var drugPriceRepository = new DrugPriceRepository(puow);
            var drugStoreRepository = new DrugStoreRepository(puow);
            var drugRepository = new DrugRepository(puow);
            var drugPriceManager = new DrugPriceManager(drugPriceRepository, drugStoreRepository, drugRepository);
            _pdfReportWriter = new PdfReportWriter(drugPriceManager);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void PdfReportWriterThrowOnNoDependenciesProvided()
        {
            var pdfReportWriter = new PdfReportWriter(null);
        }

        [TestMethod]
        public async Task PdfReportWriterWritesReport()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var path = Path.Combine(currentDirectory, "Test.pdf");
            var testDispenseInformation = DrugPriceTestEnvironment.SimpleDispenseInformation;
            await _pdfReportWriter.WriteReport(testDispenseInformation, path);
            Assert.IsTrue(File.Exists(path));
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }


    }
}
