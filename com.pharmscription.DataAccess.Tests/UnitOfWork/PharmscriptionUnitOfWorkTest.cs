using System;
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
        public async Task TestCanAttachEntity()
        {
            IPharmscriptionUnitOfWork puow = new PharmscriptionUnitOfWork();
            IPatientRepository _patientRepository = new PatientRepository(puow);
            var patient = new Patient
            {
                AhvNumber = "123",
                FirstName = "Rafael",
                BirthDate = new DateTime(1991, 03, 17)
            };
            _patientRepository.Add(patient);
            await puow.CommitAsync();
            using (var c = new PharmscriptionUnitOfWork())
            {
               c.Attach(patient);
               patient.FirstName = "Markus";
               await c.CommitAsync();
            }

            await puow.CommitAsync();
            var patientFound = await _patientRepository.GetByAhvNumber("123");

            Assert.AreEqual(patient.FirstName, patientFound.FirstName);

        }
    }
}
