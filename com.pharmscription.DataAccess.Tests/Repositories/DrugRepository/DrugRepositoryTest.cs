using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Entities.DrugEntity;
using com.pharmscription.DataAccess.Repositories.Drug;
using com.pharmscription.DataAccess.Repositories.Drug;
using com.pharmscription.DataAccess.Repositories.Patient;
using com.pharmscription.DataAccess.Tests.TestEnvironment;
using com.pharmscription.DataAccess.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.DataAccess.Tests.Repositories.DrugRepository
{
    [TestClass]
    public class DrugRepositoryTest
    {
        private IPharmscriptionUnitOfWork _puow;
        private IDrugRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            var patients = new List<Drug>
            {
                new Drug
                {
                    DrugDescription = "1001 Blattgrün Dragées",
                    NarcoticCategory = "D",
                    IsValid = true
                },
                new Drug
                {
                    DrugDescription = "1001 Blattgrün Tabletten",
                    NarcoticCategory = "D",
                    IsValid = true
                },
                new Drug
                {
                    DrugDescription = "Abilify 1 mg/ml, Sirup",
                    NarcoticCategory = "B",
                    IsValid = true
                },
                new Drug
                {
                    DrugDescription = "Abilify 10 mg, Schmerztabletten",
                    NarcoticCategory = "B",
                    IsValid = true
                },
                new Drug
                {
                    DrugDescription = "Advagraf 5 mg, Retardkapseln",
                    NarcoticCategory = "A",
                    IsValid = true
                }
            };
            var mockSet = TestEnvironmentHelper.GetMockedAsyncProviderDbSet(patients);

            var mockPuow = TestEnvironmentHelper.GetMockedDataContext();
            mockPuow.Setup(m => m.Drugs).Returns(mockSet.Object);
            mockPuow.Setup(m => m.CreateSet<Drug>()).Returns(mockSet.Object);
            _puow = mockPuow.Object;
            _repository = new DataAccess.Repositories.Drug.DrugRepository(_puow);
        }

        [TestCleanup]
        public void Cleanup()
        {

            IPharmscriptionUnitOfWork puow = new PharmscriptionUnitOfWork();
            foreach (var id in puow.Drugs.Select(e => e.Id))
            {
                var entity = new DataAccess.Entities.PatientEntity.Patient { Id = id };
                puow.Patients.Attach(entity);
                puow.Patients.Remove(entity);
            }
            puow.Commit();
        }

        [TestMethod]
        public async Task TestCanSearchByName()
        {
            var drugs = (await _repository.SearchByName("Blattgrün")).ToList();
            Assert.AreEqual(2, drugs.Count);
            var drug = drugs.FirstOrDefault();
            Assert.IsNotNull(drug);
            Assert.AreEqual("1001 Blattgrün Dragées", drug.DrugDescription);
        }

        [TestMethod]
        public async Task TestCanSearchByNameDoesNotExistReturnsEmpty()
        {
            var drugs = await _repository.SearchByName("LeoLuluPl");
            Assert.IsFalse(drugs.Any());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException)) ]
        public async Task TestSearchByNameThrowNullOnNull()
        {
            var drugs = await _repository.SearchByName(null);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public async Task TestSearchByNameThrowArgumentOnEmpty()
        {
            var drugs = await _repository.SearchByName("");
        }
    }
}
