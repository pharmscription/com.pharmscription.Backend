using System.Threading.Tasks;

namespace com.pharmscription.DataAccess.Tests.TestEnvironment
{
    using System.IO;
    using System.Reflection;
    using DataAccess.UnitOfWork;

    public class DrugPriceTestEnvironment
    {
        public static async Task LoadTestDrugPrices()
        {

            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "com.pharmscription.DataAccess.Tests.script.sql";

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
    }
}
