using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.Reporting.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BusinessLogic.DrugPrice;
    using DataAccess.Entities.AddressEntity;
    using DataAccess.Entities.AddressEntity.CityCodeEntity;
    using DataAccess.Entities.CounterProposalEntity;
    using DataAccess.Entities.DispenseEntity;
    using DataAccess.Entities.DrugEntity;
    using DataAccess.Entities.DrugItemEntity;
    using DataAccess.Entities.PatientEntity;
    using DataAccess.Entities.PrescriptionEntity;
    using DataAccess.Repositories.Dispense;
    using DataAccess.Repositories.Drug;
    using DataAccess.Repositories.DrugPrice;
    using DataAccess.Repositories.DrugStore;
    using DataAccess.Repositories.Patient;
    using DataAccess.Tests.TestEnvironment;
    using DataAccess.UnitOfWork;
    using Infrastructure.EntityHelper;

    [TestClass]
    public class PdfReportWriterTest
    {
        [TestInitialize]
        public void SetUp()
        {
            DrugTestEnvironment.SeedDrugs();
            DrugStoreTestEnvironment.SeedDrugStores();
            DrugPriceTestEnvironment.SeedDrugPrices();
        }

        [TestCleanup]
        public void TearDown()
        {
            IPharmscriptionUnitOfWork puow = new PharmscriptionUnitOfWork();
            puow.ExecuteCommand("Delete From DrugPrices");
            puow.ExecuteCommand("Delete From DrugItems");
            puow.ExecuteCommand("Delete From Drugs");
        }

        [TestMethod]
        public async Task CanWrite()
        {

            var puow = new PharmscriptionUnitOfWork();
            var writer = new PdfReportWriter(new DrugPriceManager(new DrugPriceRepository(puow), new DrugStoreRepository(puow), new DrugRepository(puow)));
            var counterProposals = new List<CounterProposal>
            {
                new CounterProposal
                {
                    Message = "Hallo"
                }
            };
            var drugs = new List<DrugItem>
            {
                new DrugItem
                {
                    Quantity = 2,
                    Drug = new Drug
                    {
                        Id = new Guid("8ef38d52-4d11-c819-6e8b-08d3783dfd75"),
                        IsValid = true,
                        DrugDescription = "Aspirin"
                    }
                },
                new DrugItem
                {
                    Quantity = 3,
                    Drug = new Drug
                    {
                        Id = new Guid("6d32f5e6-3cda-c903-a925-08d3783dfd75"),
                        IsValid = true,
                        DrugDescription = "Mebucain"
                    }
                }
            };
            var prescriptionToInsert = new SinglePrescription
            {
                Id = IdentityGenerator.NewSequentialGuid(),
                CreatedDate = DateTime.Now,
                EditDate = DateTime.Now,
                IssueDate = DateTime.Now,
                IsValid = true,
                CounterProposals = counterProposals

            };
            var prescriptionDispense = new PrescriptionDispenses
            {
                Prescription = prescriptionToInsert,
                Dispenses = new List<Dispense>
                {
                    new Dispense
                    {
                        CreatedDate = DateTime.Now,
                        Date = DateTime.Now,
                        Remark = "War eine super Ausgabe",
                        DrugItems = drugs
                    }
                }
            };
            var dispenseInformation = new DispenseInformation
            {
                Patient = new Patient
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
                    Insurance = "Zurich"

                },
                PrescriptionDispenseses = new List<PrescriptionDispenses>
                {
                    prescriptionDispense
                }
            };
            await writer.WriteReport(dispenseInformation, "Test");
        }

        [TestMethod]
        public async Task TestCanReport()
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
                    Drug = drugRepo.Get(new Guid("8ef38d52-4d11-c819-6e8b-08d3783dfd75"))
                },
                new DrugItem
                {
                    Quantity = 3,
                    Drug = drugRepo.Get(new Guid("6d32f5e6-3cda-c903-a925-08d3783dfd75"))
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
            await patientRepo.UnitOfWork.CommitAsync();
            var reporter = new Reporter(new PdfReportWriter(new DrugPriceManager(new DrugPriceRepository(puow),new DrugStoreRepository(puow), new DrugRepository(puow) )), new PrescriptionCrawler(new PatientRepository(puow)), new DispenseRepository(puow));
            await reporter.WriteReports();
        }
    }
}
