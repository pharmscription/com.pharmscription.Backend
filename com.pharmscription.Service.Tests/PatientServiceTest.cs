namespace com.pharmscription.Service.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using com.pharmscription.BusinessLogic.Patient;
    using com.pharmscription.Infrastructure.Dto;
    using com.pharmscription.Infrastructure.Exception;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    [ExcludeFromCodeCoverage]
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
            Location = "Rapperswil-Jona",
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
        
        [TestMethod]
        public async Task TestGetPatient()
        {
            mock.Setup(m => m.GetById(CorrectId)).Returns(Task.Run(() => _patient));
            PatientDto dto = await service.GetPatient(CorrectId);
            Assert.AreEqual(_patient, dto);   
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task TestGetNonExistentPatient()
        {
            mock.Setup(m => m.GetById(WrongId)).Throws<ArgumentException>();
            await service.GetPatient(WrongId);
        }

        [TestMethod]
        public async Task TestLookupPatient()
        {
            mock.Setup(m => m.Lookup(_patient.AhvNumber)).ReturnsAsync(_patient);
            PatientDto dto = await service.LookupPatient(_patient.AhvNumber);
            Assert.AreEqual(_patient, dto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task TestLookupInvalidAhv()
        {
            mock.Setup(m => m.Lookup("123")).Throws<ArgumentException>();
            await service.LookupPatient("123");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task TestLookupNullAhv()
        {
            mock.Setup(m => m.Lookup(null)).Throws<ArgumentNullException>();
            await service.LookupPatient(null);
        }

        [TestMethod]
        public async Task TestCreatePatient()
        {
            mock.Setup(m => m.Add(_patient)).ReturnsAsync(_patient);
            PatientDto dto = await service.CreatePatient(_patient);
            Assert.AreEqual(_patient, dto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task TestCreateInvalidPatient()
        {
            var invaliDto = new PatientDto();
            mock.Setup(m => m.Add(invaliDto)).Throws<ArgumentException>();
            await service.CreatePatient(invaliDto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task TestCreateNullPatient()
        {
            mock.Setup(m => m.Add(null)).Throws<ArgumentNullException>();
            await service.CreatePatient(null);
        }

        [TestMethod]
        public async Task TestSearchPatientByAhv()
        {
            mock.Setup(m => m.Find(_patient.AhvNumber)).ReturnsAsync(_patient);
            PatientDto dto = await service.GetPatientByAhv(_patient.AhvNumber);
            Assert.AreEqual(_patient, dto);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAhvNumberException))]
        public async Task TestSearchInvalidAhvNubmer()
        {
            mock.Setup(m => m.Find("123")).Throws<InvalidAhvNumberException>();
            await service.GetPatientByAhv("123");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task TestSearchNullAhvNumber()
        {
            mock.Setup(m => m.Find(null)).Throws<ArgumentNullException>();
            await service.GetPatientByAhv(null);
        }

        [TestMethod]
        public async Task TestSearchUnsucessful()
        {
            mock.Setup(m => m.Find(_patient.AhvNumber)).ReturnsAsync(null);
            PatientDto dto = await service.GetPatientByAhv(_patient.AhvNumber);
            Assert.IsNull(dto);
        }
    }
}
