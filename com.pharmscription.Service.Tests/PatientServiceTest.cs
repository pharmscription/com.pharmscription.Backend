using com.pharmscription.BusinessLogic.Drug;

namespace com.pharmscription.Service.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.ServiceModel.Web;
    using System.Threading.Tasks;

    using BusinessLogic.Patient;

    using com.pharmscription.BusinessLogic.Prescription;

    using Infrastructure.Dto;
    using Infrastructure.Exception;

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
            var mock2 = new Mock<IDrugManager>();
            var mock3 = new Mock<IPrescriptionManager>();
            service = new RestService(mock.Object, mock2.Object, mock3.Object);
            
        }
        
        [TestMethod]
        public async Task TestGetPatient()
        {
            mock.Setup(m => m.GetById(CorrectId)).Returns(Task.Run(() => _patient));
            PatientDto dto = await service.GetPatient(CorrectId);
            Assert.AreEqual(_patient, dto);   
        }

        [TestMethod]
        public async Task TestGetNonExistentPatient()
        {
            mock.Setup(m => m.GetById(WrongId)).Throws<NotFoundException>();
            try
            {
                await service.GetPatient(WrongId);
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.NotFound, e.StatusCode);
            }
        }

        [TestMethod]
        public async Task TestLookupPatient()
        {
            mock.Setup(m => m.Lookup(_patient.AhvNumber)).ReturnsAsync(_patient);
            PatientDto dto = await service.LookupPatient(_patient.AhvNumber);
            Assert.AreEqual(_patient, dto);
        }

        [TestMethod]
        public async Task TestLookupPatientNotFound()
        {
            mock.Setup(m => m.Lookup("756.1234.123.1234")).Throws<NotFoundException>();
            try
            {
                await service.LookupPatient("756.1234.123.1234");
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.NotFound, e.StatusCode);
            }
        }
        [TestMethod]
        public async Task TestLookupInvalidAhv()
        {
            mock.Setup(m => m.Lookup("123")).Throws<InvalidArgumentException>();
            try
            {
                await service.LookupPatient("123");
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, e.StatusCode);
            }
        }

        [TestMethod]
        public async Task TestLookupNullAhv()
        {
            mock.Setup(m => m.Lookup(null)).Throws<ArgumentNullException>();
            try
            {
                await service.LookupPatient(null);
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, e.StatusCode);
            } 
        }

        [TestMethod]
        public async Task TestCreatePatient()
        {
            mock.Setup(m => m.Add(_patient)).ReturnsAsync(_patient);
            PatientDto dto = await service.CreatePatient(_patient);
            Assert.AreEqual(_patient, dto);
        }

        [TestMethod]
        public async Task TestCreateInvalidPatient()
        {
            var invaliDto = new PatientDto();
            mock.Setup(m => m.Add(invaliDto)).Throws<InvalidArgumentException>();
            try
            {
                await service.CreatePatient(invaliDto);
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, e.StatusCode);
            }
        }

        [TestMethod]
        public async Task TestCreateNullPatient()
        {
            mock.Setup(m => m.Add(null)).Throws<ArgumentNullException>();
            try
            {
                await service.CreatePatient(null);
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, e.StatusCode);
            }
        }

        [TestMethod]
        public async Task TestSearchPatientByAhv()
        {
            mock.Setup(m => m.Find(_patient.AhvNumber)).ReturnsAsync(_patient);
            PatientDto dto = await service.GetPatientByAhv(_patient.AhvNumber);
            Assert.AreEqual(_patient, dto);
        }

        [TestMethod]
        public async Task TestSearchInvalidAhvNubmer()
        {
            mock.Setup(m => m.Find("123")).Throws<InvalidAhvNumberException>();
            try
            {
                await service.GetPatientByAhv("123");
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, e.StatusCode);
            }
        }

        [TestMethod]
        public async Task TestSearchNullAhvNumber()
        {
            mock.Setup(m => m.Find(null)).Throws<ArgumentNullException>();
            try
            {
                await service.GetPatientByAhv(null);
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, e.StatusCode);
            }
        }

        [TestMethod]
        public async Task TestSearchUnsucessful()
        {
            mock.Setup(m => m.Find(_patient.AhvNumber)).ReturnsAsync(null);
            try
            {
                await service.GetPatientByAhv(_patient.AhvNumber);
            }
            catch (WebFaultException<ErrorMessage> e)
            {
                Assert.AreEqual(HttpStatusCode.BadRequest, e.StatusCode);
            }
        }
    }
}
