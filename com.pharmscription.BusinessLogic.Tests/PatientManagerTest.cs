using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.BusinessLogic.Patient;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.DataAccess.Tests.TestEnvironment;
using com.pharmscription.DataAccess.UnitOfWork;
using com.pharmscription.Infrastructure.Dto;
using com.pharmscription.Infrastructure.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.BusinessLogic.Tests
{
    [TestClass]
    public class PatientManagerTest
    {
        private IPatientRepository _repository;
        private IPharmscriptionUnitOfWork _puow;

        private IPatientManager _patientManager;

        [TestInitialize]
        public void Initialize()
        {
            var patients = new List<DataAccess.Entities.PatientEntity.Patient>
            {
                new DataAccess.Entities.PatientEntity.Patient
                {
                    Id = Guid.Parse("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37"),
                    AhvNumber = "123",
                    FirstName = "Rafael",
                    BirthDate = new DateTime(1991, 03, 17)
                },
                new DataAccess.Entities.PatientEntity.Patient
                {
                    AhvNumber = "124",
                    FirstName = "Noah",
                    BirthDate = new DateTime(1990, 03, 17)
                },
                new DataAccess.Entities.PatientEntity.Patient
                {
                    AhvNumber = "125",
                    FirstName = "Markus",
                    BirthDate = new DateTime(1998, 03, 17)
                },
                new DataAccess.Entities.PatientEntity.Patient
                {
                    AhvNumber = "126",
                    FirstName = "Pascal",
                    BirthDate = new DateTime(1987, 03, 17)
                },
                new DataAccess.Entities.PatientEntity.Patient
                {
                    AhvNumber = "127",
                    FirstName = "Oliviero",
                    BirthDate = new DateTime(1983, 03, 17)
                }
            };
            var mockSet = TestEnvironmentHelper.GetMockedAsyncProviderDbSet(patients);

            var mockPuow = TestEnvironmentHelper.GetMockedDataContext();
            mockPuow.Setup(m => m.Patients).Returns(mockSet.Object);
            mockPuow.Setup(m => m.CreateSet<DataAccess.Entities.PatientEntity.Patient>()).Returns(mockSet.Object);
            _puow = mockPuow.Object;
            _repository = new PatientRepository(_puow);

            _patientManager = new PatientManager(_repository);
        }

        [TestCleanup]
        public void Cleanup()
        {

            IPharmscriptionUnitOfWork puow = new PharmscriptionUnitOfWork();

            foreach (var id in puow.Patients.Select(e => e.Id))
            {
                var entity = new DataAccess.Entities.PatientEntity.Patient { Id = id };
                puow.Patients.Attach(entity);
                puow.Patients.Remove(entity);
            }
            puow.Commit();
        }

        //[TestMethod]
        //[ExpectedException(typeof(InvalidAhvNumberException))]
        public async Task InvalidAhvNumberTest()
        {
            DateTime birthDate = new DateTime(2000, 10, 10);
            var patientDto = new PatientDto
            {
                FirstName = "Max",
                LastName = "Müller",
                Address = new AddressDto
                {
                    Street = "Bergstrasse",
                    Number = "100",
                    CityCode = "8000",
                    City = " Zürich",
                    StreetExtension = "Postfach 1234"
                },
                AhvNumber = "1231234123412",
                BirthDate = birthDate,
                InsuranceNumber = "Zurich-12345",
                PhoneNumber = "056 217 21 21",
                Insurance = "Zurich"
            };

            var patient = await _patientManager.Add(patientDto);

            Assert.IsNotNull(patient);
        }

        [TestMethod]
        public async Task AddPatientTest()
        {
            DateTime birthDate = new DateTime(2000, 10, 10);
            var patientDto = new PatientDto
            {
                FirstName = "Max",
                LastName = "Müller",
                Address = new AddressDto
                {
                    Street = "Bergstrasse",
                    Number = "100",
                    CityCode = "8000",
                    City = " Zürich",
                    StreetExtension = "Postfach 1234"
                },
                AhvNumber = "7561234567897",
                BirthDate = birthDate,
                InsuranceNumber = "Zurich-12345",
                PhoneNumber = "056 217 21 21",
                Insurance = "Zurich"
            };

            var patient = await _patientManager.Add(patientDto);

            Assert.IsNotNull(patient);

        }


        [TestMethod]
        public async Task GetByIdTest()
        {
            var patient = await _patientManager.GetById("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e37");
            Assert.IsNotNull(patient);
            Assert.AreEqual("123", patient.AhvNumber);
        }

        [TestMethod]
        public async Task FindTest()
        {
            var patient = await _patientManager.Find("123");
            Assert.IsNotNull(patient);
            Assert.AreEqual("Rafael", patient.FirstName);
        }

        [TestMethod]
        public async Task LookupFindTest()
        {
            var patient = await _patientManager.Lookup("7561234567897");
            Assert.IsNotNull(patient);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAhvNumberException))]
        public async Task LookupNotFindTest()
        {
            var patient = await _patientManager.Lookup("notfound");
            Assert.IsNull(patient);
            
        }
    }
}
