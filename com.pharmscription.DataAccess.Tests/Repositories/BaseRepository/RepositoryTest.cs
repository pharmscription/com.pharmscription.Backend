using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Repositories.BaseRepository;
using com.pharmscription.DataAccess.SharedInterfaces;
using com.pharmscription.DataAccess.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
            var mockSet = new Mock<DbSet<DataAccess.Entities.PatientEntity.Patient>>();
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
            mockSet.As<IQueryable<DataAccess.Entities.PatientEntity.Patient>>().Setup(m => m.Provider).Returns(patients.AsQueryable().Provider);
            mockSet.As<IQueryable<DataAccess.Entities.PatientEntity.Patient>>().Setup(m => m.Expression).Returns(patients.AsQueryable().Expression);
            mockSet.As<IQueryable<DataAccess.Entities.PatientEntity.Patient>>().Setup(m => m.ElementType).Returns(patients.AsQueryable().ElementType);
            mockSet.As<IQueryable<DataAccess.Entities.PatientEntity.Patient>>().Setup(m => m.GetEnumerator()).Returns(() => patients.GetEnumerator());

            mockSet.Setup(d => d.Add(It.IsAny<DataAccess.Entities.PatientEntity.Patient>())).Callback<DataAccess.Entities.PatientEntity.Patient>(patients.Add);
            mockSet.Setup(d => d.AddRange(It.IsAny<IEnumerable<DataAccess.Entities.PatientEntity.Patient>>()))
                .Callback((IEnumerable<DataAccess.Entities.PatientEntity.Patient> l) => patients.AddRange(l));
            mockSet.Setup(d => d.Remove(It.IsAny<DataAccess.Entities.PatientEntity.Patient>()))
                .Callback((DataAccess.Entities.PatientEntity.Patient el) => patients.Remove(el));
            mockSet.Setup(d => d.RemoveRange(It.IsAny<IEnumerable<DataAccess.Entities.PatientEntity.Patient>>()))
                .Callback((IEnumerable<DataAccess.Entities.PatientEntity.Patient> l) => patients.RemoveAll(l.Contains));
            //mockSet.Setup(d => d.Find(It.IsAny<Guid>())).Returns((Guid id) => patients.First(e => e.Id == id));

            mockSet.Setup(d => d.Find(It.IsAny<object[]>()))
                .Returns<object[]>(ids => patients.FirstOrDefault(d => d.Id == (Guid) ids[0]));

            var mockPuow = new Mock<PharmscriptionUnitOfWork>();
            mockPuow.Setup(m => m.Patients).Returns(mockSet.Object);
            mockPuow.Setup(m => m.CreateSet<DataAccess.Entities.PatientEntity.Patient>()).Returns(mockSet.Object);
            _puow = mockPuow.Object;
            _repository = new Repository<DataAccess.Entities.PatientEntity.Patient>(_puow);
            _puow.Commit();
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
        public void TestDoesTrack()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestTrackThrowsNullPointer()
        {
            _repository.TrackItem(null);
        }

        [TestMethod]
        public void TestDoesUntrack()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestUntrackThrowsNullPointer()
        {
            _repository.UntrackItem(null);
        }

        [TestMethod]
        public void TestDoesSetModify()
        {
            
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
        public void TestDoesGetAllAsNoTracking()
        {

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

        }

        [TestMethod]
        public void TestGetEntitiesFiltered()
        {
            var patientsOlderThan1990 = _repository.GetFiltered(patient => patient.BirthDate > new DateTime(1990, 1, 1)).ToList();
            Assert.AreEqual(3, patientsOlderThan1990.Count);
        }

        [TestMethod]
        public void TestMergeEntities()
        {

        }

    }
}
