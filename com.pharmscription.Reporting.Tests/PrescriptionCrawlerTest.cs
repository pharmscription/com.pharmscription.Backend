using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.Reporting.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using BusinessLogic.Converter;
    using DataAccess.Repositories.Patient;
    using DataAccess.Tests.TestEnvironment;
    using Infrastructure.Exception;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PrescriptionCrawlerTest
    {
        private PrescriptionCrawler _prescriptionCrawler;
        private IPatientRepository _patientRepository;
        [TestInitialize]
        public void SetUp()
        {
            _patientRepository = PatientTestEnvironment.GetMockedPatientRepository().Object;
            _prescriptionCrawler = new PrescriptionCrawler(_patientRepository);    
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void TestPrescriptionCrawlerThrowsOnDependenciesNotGiven()
        {
            var crawler = new PrescriptionCrawler(null);
        }

        [TestMethod]
        public async Task TestGetRawDataReturnEmptyListOnNothingToUpdate()
        {
            var patientsToReport = await _prescriptionCrawler.GetRawDataForReport();
            Assert.IsNotNull(patientsToReport);
            Assert.IsFalse(patientsToReport.Any());
        }

        [TestMethod]

        public async Task TestGetsCorrectRawData()
        {
            _patientRepository.Add(PatientTestEnvironment.PatientWithDetailsAndOpenDispenses);
            _patientRepository.UnitOfWork.Commit();
            var reports = await _prescriptionCrawler.GetRawDataForReport();
            Assert.IsNotNull(reports);
            Assert.IsTrue(reports.Any());
            Assert.AreEqual(1, reports.Count);
            var report = reports.FirstOrDefault();
            Assert.IsNotNull(report);
            Assert.IsTrue(PatientTestEnvironment.PatientWithDetailsAndOpenDispenses.Equals(report.Patient));
            var prescriptionDispenses = report.PrescriptionDispenseses;
            Assert.IsNotNull(prescriptionDispenses);
            var prescriptionDispense = prescriptionDispenses.FirstOrDefault();
            Assert.IsNotNull(prescriptionDispense);
            var prescription = prescriptionDispense.Prescription;
            Assert.IsTrue(prescription.EntityEqualsDto(PatientTestEnvironment.PatientWithDetailsAndOpenDispenses.Prescriptions.FirstOrDefault().ConvertToDto()));
            Assert.AreEqual(1, prescription.Dispenses.Count);
        }
        
    }
}
