using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Repositories.BaseRepository;
using com.pharmscription.DataAccess.SharedInterfaces;
using com.pharmscription.DataAccess.Tests.TestEnvironment;
using com.pharmscription.DataAccess.UnitOfWork;
using com.pharmscription.Infrastructure.EntityHelper;
using com.pharmscription.Infrastructure.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.DataAccess.Tests.Repositories.BaseRepository
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class RepositoryTest
    {
        private IRepository<DataAccess.Entities.PatientEntity.Patient> _repository;

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
            var puow = mockPuow.Object;
            _repository = new Repository<DataAccess.Entities.PatientEntity.Patient>(puow);
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
        [ExpectedException(typeof(InvalidArgumentException))]
        public void TestConstructorThrowsInvalidArgumentException()
        {
            var repo = new Repository<DataAccess.Entities.PatientEntity.Patient>(null);
        }

        [TestMethod]
        public async Task TestDoesAdd()
        {
            var patient = new DataAccess.Entities.PatientEntity.Patient
            {
                AhvNumber = "2345",
                FirstName = "Ueli",
                LastName = "Büsi",
                BirthDate = new DateTime(1991, 03, 17)
            };
            _repository.Add(patient);
            await _repository.UnitOfWork.CommitAsync();
            var patients = _repository.GetAll().ToList();
            var patientFound = patients.FirstOrDefault(e => e.AhvNumber == patient.AhvNumber);
            Assert.IsNotNull(patientFound);
            Assert.AreEqual(patient.FirstName, patientFound.FirstName);


        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void TestAddThrowsInvalidArgument()
        {
            _repository.Add(null);
        }

        [TestMethod]
        public async Task TestDoesRemove()
        {
            var patientToRemove = _repository.GetAll().FirstOrDefault(e => e.AhvNumber == "123");
            _repository.Remove(patientToRemove);
            await _repository.UnitOfWork.CommitAsync();

            var patients = _repository.GetAll().ToList();
            var patientRemoved = patients.FirstOrDefault(e => e.AhvNumber == "123");
            Assert.IsNull(patientRemoved);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void TestRemoveThrowsInvalidArgument()
        {
            _repository.Remove(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void TestTrackThrowsNullPointer()
        {
            _repository.TrackItem(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void TestUntrackThrowsInvalidArgument()
        {
            _repository.UntrackItem(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void TestSetModifyhrowsInvalidArgument()
        {
            _repository.Modify(null);
        }

        [TestMethod]
        public async Task TestDoesGetById()
        {
            var patientToFind = _repository.GetAll().FirstOrDefault();
            Assert.IsNotNull(patientToFind);
            patientToFind.Id = Guid.NewGuid();
            await _repository.UnitOfWork.CommitAsync();
            var patientFound = _repository.Get(patientToFind.Id);
            Assert.AreEqual(patientToFind, patientFound);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestGetOrThrowThrowsOnNotFound()
        { 
            await _repository.GetAsyncOrThrow(IdentityGenerator.NewSequentialGuid());
        }

        [TestMethod]
        public async Task TestDoesGetOrThrowById()
        {
            var patientToFind = _repository.GetAll().FirstOrDefault();
            Assert.IsNotNull(patientToFind);
            patientToFind.Id = Guid.NewGuid();
            await _repository.UnitOfWork.CommitAsync();
            var patientFound = await _repository.GetAsyncOrThrow(patientToFind.Id);
            Assert.AreEqual(patientToFind, patientFound);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundException))]
        public async Task TestCheckIfEntityExistsThrowsOnNotFound()
        {
            await _repository.CheckIfEntityExists(IdentityGenerator.NewSequentialGuid());
        }

        [TestMethod]
        public async Task TestCheckIfEntityExists()
        {
            var patientToFind = _repository.GetAll().FirstOrDefault();
            Assert.IsNotNull(patientToFind);
            patientToFind.Id = Guid.NewGuid();
            await _repository.UnitOfWork.CommitAsync();
            await _repository.CheckIfEntityExists(patientToFind.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void TestGetByIdThrowsInvalidArgumentOnNull()
        {
            _repository.Get(Guid.Empty);
        }

        [TestMethod]
        public async Task TestDoesFindById()
        {
            var patientToFind = _repository.GetAll().FirstOrDefault();
            Assert.IsNotNull(patientToFind);
            patientToFind.Id = Guid.NewGuid();
            await _repository.UnitOfWork.CommitAsync();
            var patientFound = _repository.Find(patientToFind.Id).FirstOrDefault();
            Assert.AreEqual(patientToFind, patientFound);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void TestFindByIdThrowsInvalidArgumentOnNull()
        {
            _repository.Find(Guid.Empty);
        }

        [TestMethod]
        public void TestDoesGetAll()
        {
            var patients = _repository.GetAll().ToList();
            foreach (var patient in patients)
            {
                Assert.IsNotNull(patient.BirthDate);
            }
            Assert.AreEqual(5, patients.Count);
        }

        [TestMethod]
        public async Task TestDoesGetAllAsNoTracking()
        {
            var patientToChange = _repository.GetAllAsNoTracking().ToList().FirstOrDefault();
            Assert.IsNotNull(patientToChange);
            patientToChange.FirstName = "Superman";
            await _repository.UnitOfWork.CommitAsync();
            var patientUnchanged = _repository.GetAll().ToList().FirstOrDefault();
            Assert.IsNotNull(patientUnchanged);
            Assert.AreNotEqual(patientToChange.FirstName, patientUnchanged.FirstName);
        }

        [TestMethod]
        public void TestCount()
        {
            var count = _repository.Count();
            Assert.AreEqual(5, count);
        }

        [TestMethod]
        public void TestCountWithPredicate()
        {
            var patientsYoungerThan1990Count = _repository.Count(patient => patient.BirthDate > new DateTime(1990, 1, 1));
            Assert.AreEqual(3, patientsYoungerThan1990Count);
        }

        [TestMethod]
        public void TestGetEntitiesPaged()
        {
            var pagedEntities = _repository.GetPaged(1, 2, patient => patient.BirthDate, true).ToList();

            Assert.AreEqual(2, pagedEntities.Count);
            var firstEntity = pagedEntities.FirstOrDefault();
            Assert.IsNotNull(firstEntity);
            Assert.AreEqual("Noah", firstEntity.FirstName);
        }

        [TestMethod]
        public void TestGetEntitiesPagedReversed()
        {
            var pagedEntites = _repository.GetPaged(0, 5, patient => patient.BirthDate, false).ToList();
            var firstEntity = pagedEntites.FirstOrDefault();
            Assert.IsNotNull(firstEntity);
            Assert.AreEqual("Markus", firstEntity.FirstName);
        }
        [TestMethod]
        public void TestGetEntitiesFiltered()
        {
            var patientsOlderThan1990 = _repository.GetFiltered(patient => patient.BirthDate > new DateTime(1990, 1, 1)).ToList();
            Assert.AreEqual(3, patientsOlderThan1990.Count);
        }
    }
}
