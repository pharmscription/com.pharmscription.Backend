using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.pharmscription.Infrastructure.AddressEntity;
using com.pharmscription.Infrastructure.BaseEntity;

namespace com.pharmscription.Infrastructure.DoctorEntity
{
    class Doctor: Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address Address { get; set; }
        public string ZsrNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
    }
}
