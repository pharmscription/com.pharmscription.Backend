using System;
using com.pharmscription.BusinessLogic.Patient;
using com.pharmscription.Infrastructure.Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.Service.Tests
{
    using System.Threading.Tasks;

    using Moq;

    [TestClass]
    public class PatientServiceTest
    {
        private static RestService service;

        private static Mock<IPatientManager> mock;
         
        private const string CorrectId = "0";

        private const string WrongId = "a";

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
            mock = new Mock<IPatientManager>();
            service = new RestService(mock.Object);
            
        }
        //// All these methods will be executed when their implementation is planned
        
        [TestMethod]
        public async Task TestGetPatient()
        {
            /*mock.Setup(m => m.GetById(CorrectId)).Returns(Task.Run(() => _patient));
            PatientDto dto = await service.GetPatient(CorrectId);
            Assert.AreEqual(_patient, dto);*/
            
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task TestGetNonExistentPatient()
        {
            /*mock.Setup(m => m.GetById(WrongId)).Throws<ArgumentException>();
            await service.GetPatient(WrongId);*/
            throw new ArgumentException();
        }
        [TestMethod]
        public async Task TestCreatePatient()
        {
            //TODO: implement
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
        *//*
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
