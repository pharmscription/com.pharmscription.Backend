

namespace com.pharmscription.DataAccess.DatabaseSeeder
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using Infrastructure.Exception;
    using UnitOfWork;

    public enum Seeds
    {
        Addresses,
        Drugs,
        DrugPrices,
        DrugStores
    }

    public class DatabaseSeeder
    {
        private static readonly Dictionary<Seeds, string> FileNames = new Dictionary<Seeds, string>
        {
            {Seeds.Addresses, "addresses.sql" },
            { Seeds.Drugs, "drugs.sql"},
            { Seeds.DrugStores, "drugStores.sql"},
            { Seeds.DrugPrices, "drugPrices.sql"}
        };

        private const string ResourceFolder = "com.pharmscription.DataAccess.";

        private static string GetRessourceName(Seeds seed)
        {
            if (!FileNames.ContainsKey(seed))
            {
                throw new InvalidArgumentException("No Seed for such a table configured yet");
            }
            var resourceName = ResourceFolder + FileNames[seed];
            return resourceName;
        }

        public static async Task SeedDataTableAsync(Seeds seed)
        {
            var resourceName = GetRessourceName(seed);
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            var command = await reader.ReadToEndAsync();
                            var puow = new PharmscriptionUnitOfWork();
                            puow.ExecuteCommand(command);
                            await puow.CommitAsync();
                        }
                    }
                }
            }
        }

        public static void SeedDataTable(Seeds seed)
        {
            var resourceName = GetRessourceName(seed);
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            var command = reader.ReadToEnd();
                            var puow = new PharmscriptionUnitOfWork();
                            puow.ExecuteCommand(command);
                            puow.Commit();
                        }
                    }
                }
            }
        }
    }
}
