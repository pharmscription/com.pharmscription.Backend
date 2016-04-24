using System;
using System.Threading.Tasks;
using System.Web.Http;
using com.pharmscription.BusinessLogic.Patient;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.DataAccess.UnitOfWork;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service.Controllers;

namespace Service.Tests.Controllers
{

    [TestClass]
    public class PatientControllerTest
    {

        private PatientController _patientController;
        [TestInitialize]
        public void SetUp()
        {
            IPharmscriptionUnitOfWork puow = new PharmscriptionUnitOfWork();
            IPatientRepository repo = new PatientRepository(puow);
            IPatientManager patientManager = new PatientManager(repo);
            _patientController = new PatientController(patientManager);

        }


        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public async Task TestGetByIdThrowsOnGarbage()
        {
            await _patientController.GetById("ksadfksdf");
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
            var ahvNumber = "756.1234.5678.97";
            var patientDto = new PatientDto
            {
                BirthDateStr = DateTime.Now.ToString("dd.MM.yyyy"),
                FirstName = "Rafael",
                LastName = "Krucker",
                AhvNumber = ahvNumber
            };
            await _patientController.Add(patientDto);
            var patientInserted = (await _patientController.GetByAhv(ahvNumber)).Content;
            Assert.IsNotNull(patientInserted);
            Assert.AreEqual(patientDto.FirstName, patientInserted.FirstName);
        }
    }
}
