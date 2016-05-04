using com.pharmscription.DataAccess.Entities.PrescriptionEntity;

namespace com.pharmscription.DataAccess.Entities.DispenseEntity
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using BaseEntity;
    using DrugItemEntity;
    using SharedInterfaces;
    [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
    public class Dispense : Entity, ICloneable<Dispense>
    {
        public DateTime Date { get; set; }
        public string Remark { get; set; }
        public virtual ICollection<DrugItem> DrugItems { get; set; }

        public virtual StandingPrescription StandingPrescription { get; set; }
        public virtual SinglePrescription SinglePrescription { get; set; }
        public virtual Prescription Prescription
        {
            get;
            set;
        }

        public Dispense Clone()
        {
            return new Dispense
                       {
                           Date = Date,
                           Remark = Remark,
                           DrugItems = DrugItems.Select(di => di.Clone()).ToList()
                       };
        }
    }
}