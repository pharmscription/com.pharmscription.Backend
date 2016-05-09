using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.pharmscription.DataAccess.Tests.TestEnvironment
{
    using DataAccess.Entities.AddressEntity;
    using DataAccess.Entities.AddressEntity.CityCodeEntity;
    using DataAccess.Entities.DrugStoreEntity;

    public class DrugStoreTestEnvironment
    {
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
