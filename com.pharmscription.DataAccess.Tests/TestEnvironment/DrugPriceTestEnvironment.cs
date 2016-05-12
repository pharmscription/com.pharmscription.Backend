using System.Threading.Tasks;

namespace com.pharmscription.DataAccess.Tests.TestEnvironment
{
    using System.Linq;
    using DataAccess.UnitOfWork;

    public class DrugPriceTestEnvironment
    {
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
    }
}
