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
    using System.Net;
    using System.Web.Mvc;

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
        public async Task TestGetByIdThrowsOnGarbage()
        {
            var result = (HttpStatusCodeResult)await _patientController.GetById("ksadfksdf");
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetById()
        {
            var patientInserted = (PatientDto)((JsonResult)await _patientController.Add(TestPatientDto)).Data;
            var patientFound =
                (PatientDto)((JsonResult)await _patientController.GetById(patientInserted.Id)).Data;
            Assert.IsNotNull(patientFound);
            Assert.AreEqual(TestPatientDto.FirstName, patientFound.FirstName);
        }

        [TestMethod]
        public async Task TestAddPatientThrowsOnNull()
        {
            var result = (HttpStatusCodeResult)await _patientController.Add(null);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestAddPatient()
        {
            await _patientController.Add(TestPatientDto);
            var patientInserted = (PatientDto)((JsonResult)await _patientController.GetByAhv(TestAhvNumber)).Data;
            Assert.IsNotNull(patientInserted);
            Assert.AreEqual(TestPatientDto.FirstName, patientInserted.FirstName);
        }

        [TestMethod]
        public async Task TestAddPatientThrowsIfBirthDateNoneGiven()
        {
            var patientDto = new PatientDto
            {
                FirstName = "Jessi",
                LastName = "SuperJessi",
                AhvNumber = TestAhvNumber
            };
            var result = (HttpStatusCodeResult)await _patientController.Add(patientDto);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            /*var patientInsertedFresh = (PatientDto)((JsonResult)await _patientController.GetById(patientInserted.Id)).Data;
            Assert.IsNotNull(patientInsertedFresh);
            Assert.AreEqual(patientDto.FirstName, patientInsertedFresh.FirstName);*/


        }

        [TestMethod]
        public async Task TestGetByAhvThrowsOnGarbage()
        {
            var result = (HttpStatusCodeResult)await _patientController.GetByAhv(null);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetByAhv()
        {
            var patientInserted = (PatientDto)((JsonResult)await _patientController.Add(TestPatientDto)).Data;
            var patientFound =
                (PatientDto)((JsonResult)await _patientController.GetByAhv(patientInserted.AhvNumber)).Data;
            Assert.IsNotNull(patientFound);
            Assert.AreEqual(TestPatientDto.FirstName, patientFound.FirstName);
        }

        [TestMethod]
        public async Task TestLookupByAhvThrowsOnGarbage()
        {
            var result = (HttpStatusCodeResult)await _patientController.LookupByAhvNumber(null);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestLookupByAhv()
        {
            var patientInserted = (PatientDto)((JsonResult)await _patientController.Add(TestPatientDto)).Data;
            var patientFound = (PatientDto)((JsonResult)await _patientController.LookupByAhvNumber(patientInserted.AhvNumber)).Data;
            Assert.IsNotNull(patientFound);
            Assert.AreEqual("Max", patientFound.FirstName);
        }
    }
}
