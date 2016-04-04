using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Repositories.BaseRepository;
using com.pharmscription.DataAccess.SharedInterfaces;
using com.pharmscription.DataAccess.Tests.TestEnvironment;
using com.pharmscription.DataAccess.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.DataAccess.Tests.Repositories.BaseRepository
{
    [TestClass]
    public class RepositoryTest
    {
        private IRepository<DataAccess.Entities.PatientEntity.Patient> _repository;
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
            var mockSet = TestEnvironmentHelper.GetMockedDbSet(patients);

            var mockPuow = TestEnvironmentHelper.GetMockedDataContext();
            mockPuow.Setup(m => m.Patients).Returns(mockSet.Object);
            mockPuow.Setup(m => m.CreateSet<DataAccess.Entities.PatientEntity.Patient>()).Returns(mockSet.Object);
            _puow = mockPuow.Object;
            _repository = new Repository<DataAccess.Entities.PatientEntity.Patient>(_puow);
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
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestConstructorThrowsArgumentNullException()
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
            await _puow.CommitAsync();
            var patients = _puow.Patients.ToList();
            var patientFound = patients.FirstOrDefault(e => e.AhvNumber == "2345");
            Assert.IsNotNull(patientFound);
            Assert.AreEqual("Ueli", patientFound.FirstName);


        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestAddThrowsNullPointer()
        {
            _repository.Add(null);
        }

        [TestMethod]
        public async Task TestDoesRemove()
        {
            var patientToRemove = _puow.Patients.FirstOrDefault(e => e.AhvNumber == "123");
            _repository.Remove(patientToRemove);
            await _puow.CommitAsync();

            var patients = _puow.Patients.ToList();
            var patientRemoved = patients.FirstOrDefault(e => e.AhvNumber == "123");
            Assert.IsNull(patientRemoved);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestRemoveThrowsNullPointer()
        {
            _repository.Remove(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestTrackThrowsNullPointer()
        {
            _repository.TrackItem(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestUntrackThrowsNullPointer()
        {
            _repository.UntrackItem(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestSetModifyhrowsNullPointer()
        {
            _repository.Modify(null);
        }

        [TestMethod]
        public async Task TestDoesGetById()
        {
            var patientToFind = _repository.GetAll().FirstOrDefault();
            Assert.IsNotNull(patientToFind);
            patientToFind.Id = Guid.NewGuid();
            await _puow.CommitAsync();
            var patientFound = _repository.Get(patientToFind.Id);
            Assert.AreEqual(patientToFind, patientFound);

        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestGetByIdThrowsNullReferenceOnNull()
        {
            _repository.Get(Guid.Empty);
        }

        [TestMethod]
        public async Task TestDoesFindById()
        {
            var patientToFind = _repository.GetAll().FirstOrDefault();
            Assert.IsNotNull(patientToFind);
            patientToFind.Id = Guid.NewGuid();
            await _puow.CommitAsync();
            var patientFound = _repository.Find(patientToFind.Id).FirstOrDefault();
            Assert.AreEqual(patientToFind, patientFound);

        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestFindByIdThrowsNullReferenceOnNull()
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
            patientToChange.FirstName = "Superman";
            await _puow.CommitAsync();
            var patientUnchanged = _repository.GetAll().ToList().FirstOrDefault();
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

            Assert.AreEqual(2, pagedEntities.Count());
            Assert.AreEqual("Noah", pagedEntities.FirstOrDefault().FirstName);
        }

        [TestMethod]
        public void TestGetEntitiesPagedReversed()
        {
            var pagedEntites = _repository.GetPaged(0, 5, patient => patient.BirthDate, false).ToList();
            Assert.AreEqual("Markus", pagedEntites.FirstOrDefault().FirstName);
        }
        [TestMethod]
        public void TestGetEntitiesFiltered()
        {
            var patientsOlderThan1990 = _repository.GetFiltered(patient => patient.BirthDate > new DateTime(1990, 1, 1)).ToList();
            Assert.AreEqual(3, patientsOlderThan1990.Count);
        }
    }
}
