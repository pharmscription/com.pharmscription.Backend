

namespace com.pharmscription.DataAccess.DatabaseSeeder
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using Infrastructure.Exception;
    using UnitOfWork;

    public enum Seed
    {
        Address,
        Drug,
        DrugPrice,
        DrugStore
    }

    [ExcludeFromCodeCoverage]
    public class DatabaseSeeder
    {
        private static readonly Dictionary<Seed, string> FileNames = new Dictionary<Seed, string>
        {
            {Seed.Address, "addresses.sql" },
            { Seed.Drug, "drugs.sql"},
            { Seed.DrugStore, "drugStores.sql"},
            { Seed.DrugPrice, "drugPrices.sql"}
        };

        private const string ResourceFolder = "com.pharmscription.DataAccess.";

        private static string GetRessourceName(Seed seed)
        {
            if (!FileNames.ContainsKey(seed))
            {
                throw new InvalidArgumentException("No Seed for such a table configured yet");
            }
            var resourceName = ResourceFolder + FileNames[seed];
            return resourceName;
        }

        public static async Task SeedDataTableAsync(Seed seed)
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

        public static void SeedDataTable(Seed seed)
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
