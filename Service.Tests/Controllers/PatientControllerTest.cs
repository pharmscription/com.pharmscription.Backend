using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using com.pharmscription.BusinessLogic.Patient;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.DataAccess.UnitOfWork;
using com.pharmscription.Infrastructure.Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.Controllers;

namespace Service.Tests.Controllers
{

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PatientControllerTest
    {

        private PatientController _patientController;

        private const string TestAhvNumber = "756.1234.5678.97";

        private static readonly PatientDto TestPatientDto = new PatientDto
        {
            BirthDateStr = DateTime.Now.ToString("dd.MM.yyyy"),
            FirstName = "Bruce",
            LastName = "Wayne",
            AhvNumber = TestAhvNumber
        };

        [TestInitialize]
        public void SetUp()
        {
            IPharmscriptionUnitOfWork puow = new PharmscriptionUnitOfWork();
            IPatientRepository repo = new PatientRepository(puow);
            IPatientManager patientManager = new PatientManager(repo);
            _patientController = new PatientController(patientManager);

        }

        [TestCleanup]
        public void TearDown()
        {
            IPharmscriptionUnitOfWork puow = new PharmscriptionUnitOfWork();
            IPatientRepository repo = new PatientRepository(puow);
            var testPatient = repo.GetAll().FirstOrDefault(e => e.AhvNumber == TestAhvNumber);
            if (testPatient != null)
            {
                repo.Remove(testPatient);
            }
            
            repo.UnitOfWork.Commit();

        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetByIdThrowsOnGarbage()
        {
            await _patientController.GetById("ksadfksdf");
        }

        [TestMethod]
        public async Task TestGetById()
        {
            var patientInserted = (await _patientController.Add(TestPatientDto)).Content;
            var patientFound = (await _patientController.GetById(patientInserted.Id)).Content;
            Assert.IsNotNull(patientFound);
            Assert.AreEqual(TestPatientDto.FirstName, patientFound.FirstName);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestAddPatientThrowsOnNull()
        {
            await _patientController.Add(null);
        }

        [TestMethod]
        public async Task TestAddPatient()
        {

            await _patientController.Add(TestPatientDto);
            var patientInserted = (await _patientController.GetByAhv(TestAhvNumber)).Content;
            Assert.IsNotNull(patientInserted);
            Assert.AreEqual(TestPatientDto.FirstName, patientInserted.FirstName);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetByAhvThrowsOnGarbage()
        {
            await _patientController.GetByAhv(null);
        }

        [TestMethod]
        public async Task TestGetByAhv()
        {
            var patientInserted = (await _patientController.Add(TestPatientDto)).Content;
            var patientFound = (await _patientController.GetByAhv(patientInserted.AhvNumber)).Content;
            Assert.IsNotNull(patientFound);
            Assert.AreEqual(TestPatientDto.FirstName, patientFound.FirstName);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestLookupByAhvThrowsOnGarbage()
        {
            await _patientController.LookupByAhvNumber(null);
        }

        [TestMethod]
        public async Task TestLookupByAhv()
        {
            var patientInserted = (await _patientController.Add(TestPatientDto)).Content;
            var patientFound = (await _patientController.LookupByAhvNumber(patientInserted.AhvNumber)).Content;
            Assert.IsNotNull(patientFound);
            Assert.AreEqual("Max", patientFound.FirstName);
        }
    }
}
