using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.pharmscription.Infrastructure.AddressEntity.CityCodeEntity;
using com.pharmscription.Infrastructure.BaseEntity;

namespace com.pharmscription.Infrastructure.AddressEntity
{
    public class Address: Entity
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public string Location { get; set; }
        public ICityCode cityCode { get; set; }
    }
}
