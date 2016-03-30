using System;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.DataAccess.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.DataAccess.Tests.Repositories.Patient
{
    [TestClass]
    public class PatientRepositoryTest
    {
        [TestInitialize()]
        public void Initialize()
        {
            
        }

        [TestCleanup()]
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
            IPharmscriptionUnitOfWork puow = new PharmscriptionUnitOfWork();
            IPatientRepository _patientRepository = new PatientRepository(puow);
            var patient = new DataAccess.Entities.PatientEntity.Patient
            {
                AhvNumber = "123",
                FirstName = "Rafael",
                LastName = "Krucker",
                BirthDate = new DateTime(1991, 03, 17)
            };
            _patientRepository.Add(patient);
            puow.Commit();
            var patientFount = await _patientRepository.GetByAhvNumber("123");
            
            Assert.AreEqual(patient.LastName, patientFount.LastName);
        }

        [TestMethod]
        public async Task TestCanFindPatientWithAhvNumberComittedAsync()
        {
            IPharmscriptionUnitOfWork puow = new PharmscriptionUnitOfWork();
            IPatientRepository _patientRepository = new PatientRepository(puow);
            var patient = new DataAccess.Entities.PatientEntity.Patient
            {
                AhvNumber = "123",
                FirstName = "Rafael",
                LastName = "Krucker",
                BirthDate = new DateTime(1991, 03, 17)
            };
            _patientRepository.Add(patient);
            await puow.CommitAsync();
            var patientFount = await _patientRepository.GetByAhvNumber("123");

            Assert.AreEqual(patient.LastName, patientFount.LastName);
        }

        [TestMethod]
        public async Task TestCanDeletePatient()
        {
            IPharmscriptionUnitOfWork puow = new PharmscriptionUnitOfWork();
            IPatientRepository _patientRepository = new PatientRepository(puow);
            var patient = new DataAccess.Entities.PatientEntity.Patient
            {
                AhvNumber = "123",
                FirstName = "Rafael",
                LastName = "Krucker",
                BirthDate = new DateTime(1991, 03, 17)
            };
            _patientRepository.Add(patient);
            await puow.CommitAsync();
            await puow.CommitAsync();
            
            var patientFound = await _patientRepository.GetByAhvNumber("123");

            _patientRepository.Remove(patientFound);
            await puow.CommitAsync();

            var patientDeleted = await _patientRepository.GetByAhvNumber("123");
            var patients = _patientRepository.GetAll().ToList();
            await puow.CommitAsync();

            Assert.IsNull(patientDeleted);
        }
    }
}
