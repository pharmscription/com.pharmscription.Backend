
namespace com.pharmscription.Reporting
{
    using System.Collections.Generic;
    using DataAccess.Entities.DispenseEntity;
    using DataAccess.Entities.PrescriptionEntity;

    public class PrescriptionDispenses
    {
        public Prescription Prescription { get; set; }
        public ICollection<Dispense> Dispenses { get; set; }
    }
}
