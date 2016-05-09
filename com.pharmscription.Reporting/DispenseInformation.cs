using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.pharmscription.Reporting
{
    using DataAccess.Entities.DispenseEntity;
    using DataAccess.Entities.PatientEntity;

    public class DispenseInformation
    {
        public Patient Patient { get; set; }
        public ICollection<PrescriptionDispenses> PrescriptionDispenseses { get; set; }
    }
}
