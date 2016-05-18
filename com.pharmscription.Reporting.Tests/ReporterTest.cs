namespace com.pharmscription.Reporting.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using BusinessLogic.DrugPrice;
    using DataAccess.Entities.AddressEntity;
    using DataAccess.Entities.AddressEntity.CityCodeEntity;
    using DataAccess.Entities.CounterProposalEntity;
    using DataAccess.Entities.DispenseEntity;
    using DataAccess.Entities.DrugItemEntity;
    using DataAccess.Entities.PatientEntity;
    using DataAccess.Entities.PrescriptionEntity;
    using DataAccess.Repositories.Dispense;
    using DataAccess.Repositories.Drug;
    using DataAccess.Repositories.DrugPrice;
    using DataAccess.Repositories.DrugStore;
    using DataAccess.Repositories.Patient;
    using DataAccess.UnitOfWork;
    using Infrastructure.EntityHelper;
    using Infrastructure.Exception;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ReporterTest
    {
        private Reporter _reporter;

        [TestInitialize]
        public void SetUp()
        {
            var puow = new PharmscriptionUnitOfWork();
            var drugRepository = new DrugRepository(puow);
            var drugPriceRepository = new DrugPriceRepository(puow);
            var drugStoreRepository = new DrugStoreRepository(puow);
            var drugPriceManager = new DrugPriceManager(drugPriceRepository, drugStoreRepository, drugRepository);
            var reportWriter = new PdfReportWriter(drugPriceManager);

            var patientRepository = new PatientRepository(puow);
            var crawler = new PrescriptionCrawler(patientRepository);
            var dispenseRepository = new DispenseRepository(puow);
            _reporter = new Reporter(reportWriter, crawler, dispenseRepository);
        }

        [TestCleanup]
        public void TearDown()
        {
            var puow = new PharmscriptionUnitOfWork();
            puow.ExecuteCommand("Delete From DrugItems");
            puow.ExecuteCommand("Delete From Dispenses");
            puow.ExecuteCommand("Delete From CounterProposals");
            puow.ExecuteCommand("Delete From Prescriptions");
            puow.ExecuteCommand("Delete From Patients");
            puow.Commit();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void ReporterThrowsOnDependenciesNotProvided()
        {
            var reporter = new Reporter(null, null, null);
        }

        [TestMethod]
        public async Task TestCrawlsAndWriterReport()
        {
            var puow = new PharmscriptionUnitOfWork();
            var drugRepo = new DrugRepository(puow);
            var counterProposals = new List<CounterProposal>
            {
                new CounterProposal
                {
                    Date = DateTime.Now,
                    Message = "Hallo"
                }
            };
            var drugs = new List<DrugItem>
            {
                new DrugItem
                {

                    Quantity = 2,
                    Drug = drugRepo.Get(new Guid("52307cd3-f3c5-c0b1-4e2c-08d37a6508cd"))
                },
                new DrugItem
                {
                    Quantity = 3,
                    Drug = drugRepo.Get(new Guid("94379638-f81b-c527-d6f9-08d37a6508cd"))
                }
            };
            var dispenses = new List<Dispense>
            {
                new Dispense
                {

                    CreatedDate = DateTime.Now,
                    Date = DateTime.Now,
                    Remark = "War eine super Ausgabe",
                    DrugItems = drugs
                }
            };
            var prescriptionToInsert = new SinglePrescription
            {
                Id = IdentityGenerator.NewSequentialGuid(),
                CreatedDate = DateTime.Now,
                EditDate = DateTime.Now,
                IssueDate = DateTime.Now,
                IsValid = true,
                CounterProposals = counterProposals,
                Dispenses = dispenses,
                DrugItems = drugs
            };

            var patient = new Patient
            {
                FirstName = "Max",
                LastName = "Müller",
                Address = new Address
                {
                    Street = "Bergstrasse",
                    Number = "100",
                    CityCode = SwissCityCode.CreateInstance("8080"),
                    Location = "Zürich",
                    StreetExtension = "Postfach 1234"
                },
                AhvNumber = "7561234567897",
                BirthDate = DateTime.Now,
                InsuranceNumber = "Zurich-12345",
                PhoneNumber = "056 217 21 21",
                Insurance = "Zurich",
                Prescriptions = new List<Prescription>
                {
                    prescriptionToInsert
                }

            };
            var patientRepo = new PatientRepository(puow);
            patientRepo.Add(patient);
            patientRepo.UnitOfWork.Commit();
            await _reporter.WriteReports();
            var path = Path.Combine(
            Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), patient.Insurance),
                DateTime.Now.ToString("dd.MM.yyyy")), patient.AhvNumber);
                    Directory.CreateDirectory(path);
            var endPath = Path.Combine(path, "report.pdf");
            Assert.IsTrue(File.Exists(endPath));

        }

    }
}
