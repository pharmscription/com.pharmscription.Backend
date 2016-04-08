using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.DataAccess.Tests.TestEnvironment;
using com.pharmscription.DataAccess.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.DataAccess.Tests.Repositories.Patient
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PatientRepositoryTest
    {
        private IPatientRepository _repository;
        private IPharmscriptionUnitOfWork _puow;

        [TestInitialize]
        public void Initialize()
        {
            var patients = new List<DataAccess.Entities.PatientEntity.Patient>
            {
                new DataAccess.Entities.PatientEntity.Patient
                {
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
            await _puow.CommitAsync();
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
