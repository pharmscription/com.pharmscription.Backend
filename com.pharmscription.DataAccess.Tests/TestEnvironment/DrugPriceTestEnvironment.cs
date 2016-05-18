namespace com.pharmscription.DataAccess.Tests.TestEnvironment
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess.Entities.AddressEntity;
    using DataAccess.Entities.AddressEntity.CityCodeEntity;
    using DataAccess.Entities.CounterProposalEntity;
    using DataAccess.Entities.DispenseEntity;
    using DataAccess.Entities.DrugEntity;
    using DataAccess.Entities.DrugItemEntity;
    using DataAccess.Entities.DrugPriceEntity;
    using DataAccess.Entities.PatientEntity;
    using DataAccess.Entities.PrescriptionEntity;
    using DataAccess.Repositories.DrugPrice;
    using DataAccess.UnitOfWork;
    using DatabaseSeeder;
    using Infrastructure.EntityHelper;
    using Moq;
    using Reporting;
    
    [ExcludeFromCodeCoverage]
    public class DrugPriceTestEnvironment
    {
        public static DispenseInformation SimpleDispenseInformation = new DispenseInformation
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
                new PrescriptionDispenses
                {
                    Prescription = new SinglePrescription
                    {
                        Id = IdentityGenerator.NewSequentialGuid(),
                        CreatedDate = DateTime.Now,
                        EditDate = DateTime.Now,
                        IssueDate = DateTime.Now,
                        IsValid = true,
                        CounterProposals = new List<CounterProposal>
                        {
                            new CounterProposal
                            {
                                Message = "Hallo"
                            }
                        }
                    },
                    Dispenses = new List<Dispense>
                    {
                        new Dispense
                        {
                            CreatedDate = DateTime.Now,
                            Date = DateTime.Now,
                            Remark = "War eine super Ausgabe",
                            DrugItems = new List<DrugItem>
                            {
                                new DrugItem
                                {
                                    Quantity = 2,
                                    Drug = new Drug
                                    {
                                        Id = new Guid("2f987f2e-8b79-cc9d-753f-08d37a6508cd"),
                                        IsValid = true,
                                        DrugDescription = "Aspirin"
                                    }
                                },
                                new DrugItem
                                {
                                    Quantity = 3,
                                    Drug = new Drug
                                    {
                                        Id = new Guid("94379638-f81b-c527-d6f9-08d37a6508cd"),
                                        IsValid = true,
                                        DrugDescription = "Mebucain"
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };


        public static async Task SeedDrugPricesAsync()
        {
            var puow = new PharmscriptionUnitOfWork();
            if (!puow.DrugPrices.Any())
            {
                await DatabaseSeeder.SeedDataTableAsync(Seeds.DrugPrices);
            }
        }

        public static void SeedDrugPrices()
        {
            var puow = new PharmscriptionUnitOfWork();
            if (!puow.DrugPrices.Any())
            {
                DatabaseSeeder.SeedDataTable(Seeds.DrugPrices);
            }
        }

        public static List<DrugPrice> GetTestDrugPrices()
        {
            return new List<DrugPrice>();
        }

        public static Mock<DrugPriceRepository> GetMockedDrugPriceRepository()
        {
            var testDrugPrices = GetTestDrugPrices();
            var mockSet = TestEnvironmentHelper.GetMockedAsyncProviderDbSet(testDrugPrices);
            var mockPuow = TestEnvironmentHelper.GetMockedDataContext();
            mockPuow.Setup(m => m.DrugPrices).Returns(mockSet.Object);
            var mockedRepository = TestEnvironmentHelper.CreateMockedRepository<DrugPrice, DrugPriceRepository>(
                mockPuow, mockSet, testDrugPrices);
            return mockedRepository;
        }
    }
}