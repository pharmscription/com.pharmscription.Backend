using System;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.DataAccess.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.DataAccess.Tests.Repositories.Patient
{
    [TestClass]
    public class PatientRepositoryTest
    {
        [TestMethod]
        public void TestCanFindPatientWithAhvNumber()
        {
            IPharmscriptionUnitOfWork puow = new PharmscriptionUnitOfWork();
            IPatientRepository _patientRepository = new PatientRepository(puow);
            var patient = new Entities.PatientEntity.Patient
            {
                AhvNumber = "123",
                FirstName = "Rafael",
                LastName = "Krucker"
            };
            _patientRepository.Add(patient);
            var patientFount = _patientRepository.GetByAhvNumber("123");
            Assert.AreEqual(patient.LastName, patientFount.LastName);
        }
    }
}
