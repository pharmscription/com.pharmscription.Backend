using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Entities.AddressEntity;
using com.pharmscription.DataAccess.Entities.AddressEntity.CityCodeEntity;
using com.pharmscription.DataAccess.Entities.PatientEntity;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.DataAccess.Tests.TestEnvironment;
using com.pharmscription.DataAccess.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.DataAccess.Tests.Repositories.PatientRepository
{
    using BusinessLogic.Converter;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PatientRepositoryTest
    {
        private IPatientRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            var patients = new List<Patient>
            {
                new Patient
                {
                    AhvNumber = "123",
                    FirstName = "Rafael",
                    BirthDate = new DateTime(1991, 03, 17)
                },
                new Patient
                {
                    AhvNumber = "124",
                    FirstName = "Noah",
                    BirthDate = new DateTime(1990, 03, 17)
                },
                new Patient
                {
                    AhvNumber = "125",
                    FirstName = "Markus",
                    BirthDate = new DateTime(1998, 03, 17)
                },
                new Patient
                {
                    AhvNumber = "126",
                    FirstName = "Pascal",
                    BirthDate = new DateTime(1987, 03, 17)
                },
                new Patient
                {
                    AhvNumber = "127",
                    FirstName = "Oliviero",
                    BirthDate = new DateTime(1983, 03, 17)
                }
            };
            var mockSet = TestEnvironmentHelper.GetMockedAsyncProviderDbSet(patients);

            var mockPuow = TestEnvironmentHelper.GetMockedDataContext();
            mockPuow.Setup(m => m.Patients).Returns(mockSet.Object);
            mockPuow.Setup(m => m.CreateSet<Patient>()).Returns(mockSet.Object);
            var puow = mockPuow.Object;
            _repository = new DataAccess.Repositories.Patient.PatientRepository(puow);
        }

        [TestCleanup]
        public void Cleanup()
        {

            IPharmscriptionUnitOfWork puow = new PharmscriptionUnitOfWork();

            foreach (var id in puow.Patients.Select(e => e.Id))
            {
                var entity = new Patient { Id = id };
                puow.Patients.Attach(entity);
                puow.Patients.Remove(entity);
            }
            puow.Commit();
        }

        [TestMethod]
        public async Task TestStoresPatientWithWholeAdress()
        {
            const string testAhvNumber = "7561234567897";
            var address = new Address
            {
                Location = "Wil",
                Number = "17",
                State = "Thurgau",
                Street = "Steigstrasse",
                StreetExtension = "None",
                CityCode = SwissCityCode.CreateInstance("9535")
            };
            var patient = new Patient
            {
                FirstName = "Rafael",
                LastName = "Krucker",
                Address = address,
                BirthDate = DateTime.Parse("17.03.1991"),
                AhvNumber = testAhvNumber
            };
            _repository.Add(patient);
            await _repository.UnitOfWork.CommitAsync();
            var patientInserted = await _repository.GetByAhvNumber(testAhvNumber);
            Assert.IsNotNull(patientInserted);
            var addressInserted = patientInserted.Address;
            Assert.IsTrue(address.IsEqual(addressInserted));
        }
        [TestMethod]
        public async Task TestCanFindPatientWithAhvNumber()
        {
            var patientFount = await _repository.GetByAhvNumber("123");
            Assert.AreEqual("Rafael", patientFount.FirstName);
        }

        [TestMethod]
        public async Task TestCanDeletePatient()
        {
            var patientFound = await _repository.GetByAhvNumber("123");
            _repository.Remove(patientFound);
            await _repository.UnitOfWork.CommitAsync();
            var patientDeleted = await _repository.GetByAhvNumber("123");
            Assert.IsNull(patientDeleted);
        }

        [TestMethod]
        public void TestPatientExists()
        {
            Assert.IsTrue(_repository.Exists("123"));
        }

        [TestMethod]
        public void TestPatientDoesNotExist()
        {
            Assert.IsFalse(_repository.Exists("123434566"));
        }
    }
}
