namespace com.pharmscription.DataAccess.Tests.TestEnvironment
{
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess.Entities.AddressEntity;
    using DataAccess.Entities.AddressEntity.CityCodeEntity;
    using DataAccess.Entities.DrugStoreEntity;
    using DataAccess.UnitOfWork;

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
    }
}
