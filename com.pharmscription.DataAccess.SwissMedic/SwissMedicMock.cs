﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using com.pharmscription.DataAccess.Entities.DrugEntity;
using com.pharmscription.Infrastructure.Exception;


namespace com.pharmscription.DataAccess.SwissMedic
{
    public class SwissMedicMock : ISwissMedic
    {
        private static async Task<List<Drug>> ReadDrugsFromCsv()
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "com.pharmscription.DataAccess.SwissMedic.Drugs.csv";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    return null;
                }
                    using (var reader = new StreamReader(stream))
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

        private static Drug ParseDrug(IReadOnlyList<string> line)
        {
            return new Drug
            {
                DrugDescription = line[2],
                IsValid = true,
                Composition = line[4],
                Unit = line[1],
                NarcoticCategory = line[5]
            };
        }

        public async Task<List<Drug>> SearchDrug(string partialDescription)
        {
            if (string.IsNullOrWhiteSpace(partialDescription))
            {
                throw new InvalidArgumentException("Search Text was empty or null");
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
