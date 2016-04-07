using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Entities.DrugEntity;



namespace com.pharmscription.DataAccess.SwissMedic
{
    public class SwissMedicMock : ISwissMedic
    {
        private async Task<List<Drug>> ReadDrugsFromCsv()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "com.pharmscription.DataAccess.SwissMedic.Drugs.csv";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {

                    var drugs = new List<Drug>();
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        drugs.Add(ParseDrug(line.Split(';')));
                    }
                    return drugs;
                }
            }
        }

        private Drug ParseDrug(string[] line)
        {
            return new Drug
            {
                DrugDescription = line[2],
                IsValid = true,
                NarcoticCategory = line[5]
            };
        }

        public async Task<List<Drug>> SearchDrug(string partialDescription)
        {
            if(partialDescription == null)
            {
                throw new ArgumentNullException(nameof(partialDescription));
            }
            if (string.IsNullOrWhiteSpace(partialDescription))
            {
                throw new ArgumentException("Search Text was empty");
            }
            var drugs = await ReadDrugsFromCsv();
            return drugs.Where(e => e.DrugDescription.ToLower().Contains(partialDescription.ToLower())).ToList();
        }

        public async Task<List<Drug>> GetAll()
        {
            return await ReadDrugsFromCsv();
        }
    }
}
