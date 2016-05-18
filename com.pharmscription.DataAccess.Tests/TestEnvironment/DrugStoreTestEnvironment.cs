namespace com.pharmscription.DataAccess.Tests.TestEnvironment
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess.Entities.AddressEntity;
    using DataAccess.Entities.AddressEntity.CityCodeEntity;
    using DataAccess.Entities.DrugStoreEntity;
    using DataAccess.Repositories.DrugStore;
    using DataAccess.UnitOfWork;
    using DatabaseSeeder;
    using Moq;

    [ExcludeFromCodeCoverage]
    public class DrugStoreTestEnvironment
    {
        public static async Task SeedDrugStoresAsync()
        {
            var puow = new PharmscriptionUnitOfWork();
            if (!puow.DrugStores.Any())
            {
                await DatabaseSeeder.SeedDataTableAsync(Seeds.DrugStores);
            }
        }

        public static void SeedDrugStores()
        {
            var puow = new PharmscriptionUnitOfWork();
            if (!puow.DrugStores.Any())
            {
                DatabaseSeeder.SeedDataTable(Seeds.DrugStores);
            }
        }

        public static DrugStore GetTestDrugStore()
        {
            return new DrugStore
            {
                Name = "Pharmception",
                Address = new Address
                {
                    Location = "Rapperswil",
                    Number = "18",
                    State = "St. Gallen",
                    Street = "Neue Jonastrasse",
                    StreetExtension = "a",
                    CityCode = SwissCityCode.CreateInstance("1892")
                }
            };
        }

        public static List<DrugStore> GetTestDrugStores()
        {
            return new List<DrugStore>
            {
                GetTestDrugStore()
            };
        }
        public static Mock<DrugStoreRepository> GetMockedDrugPriceRepository()
        {
            var testDrugStores = GetTestDrugStores();
            var mockSet = TestEnvironmentHelper.GetMockedAsyncProviderDbSet(testDrugStores);
            var mockPuow = TestEnvironmentHelper.GetMockedDataContext();
            mockPuow.Setup(m => m.DrugStores).Returns(mockSet.Object);
            var mockedRepository = TestEnvironmentHelper.CreateMockedRepository<DrugStore, DrugStoreRepository>(mockPuow, mockSet, testDrugStores);
            return mockedRepository;
        }
    }
}
