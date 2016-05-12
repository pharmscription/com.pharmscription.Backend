using System.Collections.Generic;

namespace com.pharmscription.Reporting
{
    using DataAccess.Entities.PatientEntity;

    public class DispenseInformation
    {
        public Patient Patient { get; set; }
        public ICollection<PrescriptionDispenses> PrescriptionDispenseses { get; set; }
    }
}
