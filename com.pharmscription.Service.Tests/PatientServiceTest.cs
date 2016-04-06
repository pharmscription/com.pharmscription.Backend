using System;
using System.IO;
using com.pharmscription.BusinessLogic.Patient;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.Service.Tests
{
    [TestClass]
    public class PatientServiceTest
    {
        private RestService _service;
        private IPatientManager _fakePatientManager;
        private const string ID = "0";

        private static readonly AddressDto ADDRESS = new AddressDto
        {
            Street = "Neue Jonastrasse",
            StreetExtension = "3. Stockwerk",
            Number = "112",
            City = "Rapperswil-Jona",
            CityCode = "8640"
        };

        private static readonly PatientDto _patient = new PatientDto
        {
            Id = "0",
            FirstName = "Oliviero",
            LastName = "Chiodo",
            Address = ADDRESS,
            AhvNumber = "10",
            BirthDate = new DateTime(1992, 7, 12),
            Insurance = "Generika",
            InsuranceNumber = "9",
            PhoneNumber = "222"
        };
        [TestInitialize]
        public void SetUp()
        {
            _fakePatientManager = new FakePatientManager();
            _service = new RestService(_fakePatientManager, null);
            
        }
        //// All these methods will be executed when their implementation is planned
        /*
        [TestMethod]
        public void TestGetPatient()
        {
            var dto = _service.GetPatient(ID);
            Assert.AreEqual(dto.Id, ID);
            
        }

        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestGetNonExistentPatient()
        {
            _service.GetAddress("5");
        }
        */
        [TestMethod]
        public void TestCreatePatient()
        {
            var newDto = _service.CreatePatient(_patient);
            Assert.AreEqual(_patient.Id, newDto.Id);
            newDto = _fakePatientManager.Lookup(_patient.AhvNumber).Result;
            Assert.AreEqual(_patient.Id, newDto.Id);
        }
        /*
        TODO: discuss how malformed data is handled
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void TestCreateInvalidPatient()
        {
            _service.CreatePatient(new PatientDto());
        }
        
        [TestMethod]
        public void TestGetAddress()
        {
            var address = _service.GetAddress("0");
            Assert.AreEqual("0", address.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void TestGetAddressFromInvalidPatient()
        {
            _service.GetAddress("5");
        }
        */
        [TestMethod]
        public void TestGetPatientByAhv()
        {
            var dto = _service.GetPatientByAhv("11");
            Assert.AreEqual("11", dto.AhvNumber);
        }

        ////[TestMethod]
        ////[ExpectedException(typeof (InvalidAhvNumberException))]
        public void TestGetPatientByInvalidAhv()
        {
            _service.GetPatientByAhv("989");
        }
        /*
        [TestMethod]
        public void TestDeletePatient()
        {
            
        }

        [TestMethod]
        public void TestDeleteInvalidPatient()
        {
            
        }
        */
    }
}
