
namespace com.pharmscription.DataAccess.Tests.TestEnvironment
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess.Entities.DrugEntity;
    using DataAccess.Repositories.Drug;
    using DataAccess.UnitOfWork;
    using DatabaseSeeder;
    using Moq;

    [ExcludeFromCodeCoverage]
    public class DrugTestEnvironment
    {
        public const string DrugOneId = "1baf86b0-1e14-4f4c-b05a-5c9dd00e8e40";
        public const string DrugOneDescription = "Aspirin";
        public const string DrugTwoId = "1bbf86b0-1f14-4f4c-c05a-5c9dd00e8e40";
        public const string DrugTwoDescription = "Mebucain";
        public const string DrugThreeId = "1bbb86b0-1e14-4f4c-b05a-5c9dd00e8e40";
        public const string DrugThreeDescription = "1001 Blattgrün Dragées";
        public const string DrugFourId = "1bbcccb0-1f14-4f4c-c05a-5c9dd00e8e40";
        public const string DrugFourDescription = "1001 Blattgrün Tabletten";
        public const string DrugFiveId = "1bafdddd-1e14-4f4c-b05a-5c9dd00e8e40";
        public const string DrugFiveDescription = "Abilify 1 mg/ml, Sirup";
        public const string DrugSixId = "aaaa86b0-1f14-4f4c-c05a-5c9dd00e8e40";
        public const string DrugSixDescription = "Abilify 10 mg, Schmerztabletten";
        public const string DrugSevenId = "222286b0-1e14-4f4c-b05a-5c9dd00e8e40";
        public const string DrugSevenDescription = "Advagraf 5 mg, Retardkapseln";

        public static List<Drug> GetTestDrugs()
        {
            return new List<Drug>
            {
              new Drug
                {
                    Id = Guid.Parse(DrugOneId),
                    IsValid = true,
                    DrugDescription = DrugOneDescription
                },
              new Drug
              {
                  Id = Guid.Parse(DrugTwoId),
                  DrugDescription = DrugTwoDescription,
                  IsValid = true
              },
              new Drug
                {
                    Id = Guid.Parse(DrugThreeId),
                    DrugDescription = DrugThreeDescription,
                    NarcoticCategory = "D",
                    IsValid = true
                },
                new Drug
                {
                    Id = Guid.Parse(DrugFourId),
                    DrugDescription = DrugFourDescription,
                    NarcoticCategory = "D",
                    IsValid = true
                },
                new Drug
                {
                    Id = Guid.Parse(DrugFiveId),
                    DrugDescription = DrugFiveDescription,
                    NarcoticCategory = "B",
                    IsValid = true
                },
                new Drug
                {
                    Id = Guid.Parse(DrugSixId),
                    DrugDescription = DrugSixDescription,
                    NarcoticCategory = "B",
                    IsValid = true
                },
                new Drug
                {
                    Id = Guid.Parse(DrugSevenId),
                    DrugDescription = DrugSevenDescription,
                    NarcoticCategory = "A",
                    IsValid = true
                }
            };
        }

        public static Mock<DrugRepository> GetMockedDrugsRepository()
        {
            var drugItems = GetTestDrugs();
            var mockSet = TestEnvironmentHelper.GetMockedAsyncProviderDbSet(drugItems);
            var mockPuow = TestEnvironmentHelper.GetMockedDataContext();
            mockPuow.Setup(m => m.Drugs).Returns(mockSet.Object);
            var mockedRepository = TestEnvironmentHelper.CreateMockedRepository<Drug, DrugRepository>(mockPuow, mockSet, drugItems);
            return mockedRepository;
        }

        public static async Task SeedDrugsAsync()
        {
            var puow = new PharmscriptionUnitOfWork();
            if (!puow.Drugs.Any())
            {
                await DatabaseSeeder.SeedDataTableAsync(Seed.Drug);
            }
        }

        public static void SeedDrugs()
        {
            var puow = new PharmscriptionUnitOfWork();
            var repo = new DrugRepository(puow);
            if (!repo.GetAll().Any())
            {
                DatabaseSeeder.SeedDataTable(Seed.Drug);
            }
        }
    }
}
