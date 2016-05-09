using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.Reporting.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DataAccess.Entities.AddressEntity;
    using DataAccess.Entities.AddressEntity.CityCodeEntity;
    using DataAccess.Entities.CounterProposalEntity;
    using DataAccess.Entities.DispenseEntity;
    using DataAccess.Entities.DrugEntity;
    using DataAccess.Entities.DrugItemEntity;
    using DataAccess.Entities.PatientEntity;
    using DataAccess.Entities.PrescriptionEntity;
    using DataAccess.Repositories.Drug;
    using DataAccess.Repositories.DrugPrice;
    using DataAccess.Repositories.DrugStore;
    using DataAccess.Tests.TestEnvironment;
    using DataAccess.UnitOfWork;
    using Infrastructure.EntityHelper;

    [TestClass]
    public class PdfReportWriterTest
    {
        [TestMethod]
        public void CanWrite()
        {
            var writer = new PdfReportWriter();
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
                        IsValid = true,
                        DrugDescription = "Aspirin"
                    }
                },
                new DrugItem
                {
                    Quantity = 3,
                    Drug = new Drug
                    {
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
            writer.WriteReport(dispenseInformation);
        }

        [TestMethod]
        public async Task PrefillsDatabase()
        {
            IPharmscriptionUnitOfWork puow = new PharmscriptionUnitOfWork();
            var drugRepo = new DrugRepository(puow);
            var drugPriceRepo = new DrugPriceRepository(puow);
            var drugStoreRepo = new DrugStoreRepository(puow);
            var mock = new DrugPriceMock(drugRepo, drugStoreRepo, drugPriceRepo);
            await mock.MockEnvironment();
        }

        [TestMethod]
        public async Task PrefillDatabase()
        {
            await DrugPriceTestEnvironment.LoadTestDrugPrices();
        }
    }
}
