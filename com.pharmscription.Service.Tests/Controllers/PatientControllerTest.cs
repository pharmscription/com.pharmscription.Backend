﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Patient;
using com.pharmscription.DataAccess.Entities.AddressEntity.CityCodeEntity;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.DataAccess.UnitOfWork;
using com.pharmscription.Infrastructure.Constants;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.EntityHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using com.pharmscription.BusinessLogic.Converter;
namespace com.pharmscription.Service.Tests.Controllers
{
    using System.Net;
    using System.Web.Mvc;
    using DataAccess.Tests.TestEnvironment;
    using Service.Controllers;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PatientControllerTest
    {

        private PatientController _patientController;
        private IPatientRepository _patientRepository;
        private const string TestAhvNumber = "756.1234.5678.97";

        private static readonly PatientDto TestPatientDto = new PatientDto
        {
            BirthDate = DateTime.Now.ToString(PharmscriptionConstants.DateFormat),
            FirstName = "Bruce",
            LastName = "Wayne",
            AhvNumber = TestAhvNumber
        };

        [ClassInitialize]
        public static void CleanDatabase(TestContext context)
        {
            var puow = new PharmscriptionUnitOfWork();

            puow.ExecuteCommand("Delete From Dispenses");
            puow.ExecuteCommand("Delete From Prescriptions");
            puow.ExecuteCommand("Delete From Patients");
            puow.Commit();
        }
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
        public async Task TestGetByIdReturnsNoContentOnNotFound()
        {
            var result = (HttpStatusCodeResult)await _patientController.GetById(IdentityGenerator.NewSequentialGuid().ToString());
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
        }

        [TestMethod]
        public async Task TestAddPatientThrowsOnNull()
        {
            var result = (HttpStatusCodeResult)await _patientController.Add(null);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestAddPatientReturnsBadRequestWhenAhvAlreadyInSystem()
        {
            await _patientController.Add(TestPatientDto);
            var result = (HttpStatusCodeResult)await _patientController.Add(TestPatientDto);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestStoresPatientWithWholeAdress()
        {
            const string testAhvNumber = "7561234567897";
            var address = new AddressDto
            {
                Location = "Wil",
                Number = "17",
                State = "Thurgau",
                Street = "Steigstrasse",
                StreetExtension = "None",
                CityCode = SwissCityCode.CreateInstance("9535").ToString()
            };
            var patient = new PatientDto
            {
                FirstName = "Rafael",
                LastName = "Krucker",
                Address = address,
                BirthDate = DateTime.Parse("17.03.1991").ToString(PharmscriptionConstants.DateFormat),
                AhvNumber = testAhvNumber
            };
            await _patientController.Add(patient);
            var patientInserted = (PatientDto)((JsonResult)await _patientController.GetByAhv(testAhvNumber)).Data;
            Assert.IsNotNull(patientInserted);
            var addressInserted = patientInserted.Address;
            Assert.IsTrue(address.ConvertToEntity().Equals(addressInserted.ConvertToEntity()));
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
        }

        [TestMethod]
        public async Task TestGetByAhvThrowsOnGarbage()
        {
            var result = (HttpStatusCodeResult)await _patientController.GetByAhv(null);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetByAhvReturnsBadRequesttMalformedAhv()
        {
            var result = (HttpStatusCodeResult)await _patientController.GetByAhv("1.2333.43.12");
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestGetByAhvReturnsNoContentOnNotFound()
        {
            var result = (HttpStatusCodeResult)await _patientController.GetByAhv(PatientTestEnvironment.AhvNumberNotInDatabase);
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
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
        public async Task TestGetByAhvReturnPatientWithAddress()
        {
            const string testAhvNumber = "7561234567897";
            var address = new AddressDto
            {
                Location = "Wil",
                Number = "17",
                State = "Thurgau",
                Street = "Steigstrasse",
                StreetExtension = "None",
                CityCode = SwissCityCode.CreateInstance("9535").ToString()
            };
            var patient = new PatientDto
            {
                FirstName = "Rafael",
                LastName = "Krucker",
                Address = address,
                BirthDate = DateTime.Parse("17.03.1991").ToString(PharmscriptionConstants.DateFormat),
                AhvNumber = testAhvNumber
            };
            await _patientController.Add(patient);
            var patientInserted = (PatientDto)((JsonResult)await _patientController.GetByAhv(testAhvNumber)).Data;
            Assert.IsNotNull(patientInserted);
            var addressInserted = patientInserted.Address;
            Assert.IsTrue(address.ConvertToEntity().Equals(addressInserted.ConvertToEntity()));
        }

        [TestMethod]
        public async Task TestLookupByAhvThrowsOnGarbage()
        {
            var result = (HttpStatusCodeResult)await _patientController.LookupByAhvNumber(null);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public async Task TestLookupReturnNoContentOnNotFound()
        {
            var result = (HttpStatusCodeResult)await _patientController.LookupByAhvNumber("7561234567897");
            Assert.AreEqual((int)HttpStatusCode.NoContent, result.StatusCode);
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
