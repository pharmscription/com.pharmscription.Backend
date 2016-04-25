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
        private IPatientRepository _patientRepository;
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
            _patientRepository = new PatientRepository(puow);
            IPatientManager patientManager = new PatientManager(_patientRepository);
            _patientController = new PatientController(patientManager);

        }

        [TestCleanup]
        public void TearDown()
        {
            var patients = _patientRepository.GetAll().ToList();
            var testPatient = patients.FirstOrDefault(e => e.AhvNumber == TestAhvNumber);
            if (testPatient != null)
            {
                _patientRepository.Remove(testPatient);
            }

            _patientRepository.UnitOfWork.Commit();

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
            var patientInserted = (PatientDto)(await _patientController.Add(TestPatientDto)).Data;
            var patientFound =
                (PatientDto)(await _patientController.GetById(patientInserted.Id)).Data;
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
            var patientInserted = (PatientDto)(await _patientController.GetByAhv(TestAhvNumber)).Data;
            Assert.IsNotNull(patientInserted);
            Assert.AreEqual(TestPatientDto.FirstName, patientInserted.FirstName);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestAddPatientThrowsIfBirthDateNoneGiven()
        {
            var patientDto = new PatientDto
            {
                FirstName = "Jessi",
                LastName = "SuperJessi",
                AhvNumber = TestAhvNumber
            };
            var patientInserted = (PatientDto)(await _patientController.Add(patientDto)).Data;
            var patientInsertedFresh = (PatientDto)(await _patientController.GetById(patientInserted.Id)).Data;
            Assert.IsNotNull(patientInsertedFresh);
            Assert.AreEqual(patientDto.FirstName, patientInsertedFresh.FirstName);

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
            var patientInserted = (PatientDto)(await _patientController.Add(TestPatientDto)).Data;
            var patientFound =
                (PatientDto)(await _patientController.GetByAhv(patientInserted.AhvNumber)).Data;
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
            var patientInserted = (PatientDto)(await _patientController.Add(TestPatientDto)).Data;
            var patientFound = (PatientDto)(await _patientController.LookupByAhvNumber(patientInserted.AhvNumber)).Data;
            Assert.IsNotNull(patientFound);
            Assert.AreEqual("Max", patientFound.FirstName);
        }
    }
}
