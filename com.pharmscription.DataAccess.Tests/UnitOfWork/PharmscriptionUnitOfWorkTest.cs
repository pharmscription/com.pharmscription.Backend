using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Entities.PatientEntity;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.DataAccess.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.DataAccess.Tests.UnitOfWork
{
    [TestClass]
    public class PharmscriptionUnitOfWorkTest
    {
        private IPharmscriptionUnitOfWork _puow;
        private IPatientRepository _patientRepository;
        private Patient _patient;
        [TestInitialize()]
        public void Initialize()
        {
            _puow = new PharmscriptionUnitOfWork();
            _patientRepository = new PatientRepository(_puow);
            _patient = new Patient
            {
                AhvNumber = "123",
                FirstName = "Rafael",
                BirthDate = new DateTime(1991, 03, 17)
            };
            _patientRepository.Add(_patient);
            _puow.Commit();
        }

        [TestCleanup()]
        public void Cleanup()
        {
            IPharmscriptionUnitOfWork puow = new PharmscriptionUnitOfWork();

            foreach (var id in puow.Patients.Select(e => e.Id))
            {
                var entity = new Patient {Id = id};
                puow.Patients.Attach(entity);
                puow.Patients.Remove(entity);
            }
            puow.Commit();
        }

        [TestMethod]
        public async Task TestCanAttachEntity()
        {
            using (var c = new PharmscriptionUnitOfWork())
            {
                c.Attach(_patient);
                _patient.FirstName = "Markus";
                await c.CommitAsync();
            }
            await _puow.CommitAsync();
            var patientFound = await _patientRepository.GetByAhvNumber("123");
            Assert.AreEqual(_patient.FirstName, patientFound.FirstName);
        }

        [TestMethod]
        public async Task TestCanDetach()
        {
            _puow.Detach(_patient);
            _patient.FirstName = "Markus";
            await _puow.CommitAsync();
            var patientFound = await _patientRepository.GetByAhvNumber("123");
            Assert.AreEqual("Rafael", patientFound.FirstName);
        }

        [TestMethod]
        public async Task TestCanCommitAndRefresh()
        {
            _patient.FirstName = "Markus";
            _puow.CommitAndRefreshChanges();
            var patientFound = await _patientRepository.GetByAhvNumber("123");
            Assert.AreEqual("Markus", patientFound.FirstName);
        }

        [TestMethod]
        public async Task TestCanSetModified()
        {
            using (var c = new PharmscriptionUnitOfWork())
            {
                _patient.FirstName = "Markus";
                c.SetModified(_patient);
                await c.CommitAsync();
            }
            await _puow.CommitAsync();
            var patientFound = await _patientRepository.GetByAhvNumber("123");
            Assert.AreEqual(_patient.FirstName, patientFound.FirstName);
        }

        [TestMethod]
        public async Task TestCanApplyCurrentValues()
        {
            var patient2 = await _patientRepository.GetByAhvNumber("123");
            await _puow.CommitAsync();
            patient2.FirstName = "Leo";
            await _puow.CommitAsync();
            _puow.ApplyCurrentValues(_patient, patient2);
            await _puow.CommitAsync();
            var patientFound = await _patientRepository.GetByAhvNumber("123");
            Assert.AreEqual("Leo", patientFound.FirstName);
        }

        [TestMethod]
        public async Task TestCanExecuteQuery()
        {
            var names = _puow.ExecuteQuery<string>("Select FirstName From Patients");
            await _puow.CommitAsync();
            Assert.AreEqual("Rafael", names.FirstOrDefault());
        }

        [TestMethod]
        public async Task TestCanExecuteCommand()
        {
            var rowsAffected = _puow.ExecuteCommand("Delete From Patients");
            await _puow.CommitAsync();
            Assert.AreEqual(rowsAffected, 1);
        }

        [TestMethod]
        public async Task TestCanRollBackChanges()
        {
            _patientRepository.Remove(_patient);
            _puow.RollbackChanges();
            await _puow.CommitAsync();
            var patients = _patientRepository.GetAll().ToList();
            Assert.AreEqual(1, patients.Count);
        }
    }
}

