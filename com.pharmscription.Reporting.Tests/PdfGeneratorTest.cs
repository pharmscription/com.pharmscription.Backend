using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.Reporting.Tests
{
    using BusinessLogic.DrugPrice;
    using DataAccess.Tests.TestEnvironment;
    using Infrastructure.Exception;

    [TestClass]
    public class PdfGeneratorTest
    {
        private PdfGenerator _pdfGenerator;

        [TestInitialize]
        public void SetUp()
        {
            var drugPriceRepository = DrugPriceTestEnvironment.GetMockedDrugPriceRepository().Object;
            var drugStoreRepository = DrugStoreTestEnvironment.GetMockedDrugPriceRepository().Object;
            var drugRepository = DrugTestEnvironment.GetMockedDrugsRepository().Object;
            var drugPriceManager = new DrugPriceManager(drugPriceRepository, drugStoreRepository, drugRepository);
            _pdfGenerator = new PdfGenerator(drugPriceManager);
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void PdfGeneratorThrowOnNoDependenciesProvided()
        {
            var generator = new PdfGenerator(null);
        }

        [TestMethod]
        public void PdfGeneratorGeneratesBaseDocument()
        {
            const string title = "Test Title";
            const string subject = "Test Subject";
            var document = _pdfGenerator.GetBaseDocument(title, subject);
            Assert.IsNotNull(document);
        }

        [TestMethod]
        public void PdfGeneratorGeneratesReport()
        {
            const string title = "Test Title";
            const string subject = "Test Subject";
            var baseDocument = _pdfGenerator.GetBaseDocument(title, subject);
            var dispenseInformation = DrugPriceTestEnvironment.SimpleDispenseInformation;
            var document = _pdfGenerator.FormatReport(baseDocument, dispenseInformation);
            Assert.IsNotNull(document);
        }
    }
}
