using System;
using System.Collections.Generic;
using System.Linq;

using com.pharmscription.DataAccess.Entities.BaseEntity;
using com.pharmscription.DataAccess.Entities.DrugItemEntity;
using com.pharmscription.DataAccess.Entities.PrescriptionEntity;
using com.pharmscription.DataAccess.SharedInterfaces;

namespace com.pharmscription.DataAccess.Entities.DispenseEntity
{
    
    public class Dispense : Entity, ICloneable<Dispense>, IEquatable<Dispense>
    {
        public DateTime? Date { get; set; }
        public string Remark { get; set; }
        public bool Reported { get; set; }
        public virtual ICollection<DrugItem> DrugItems { get; set; }

        public virtual StandingPrescription StandingPrescription { get; set; }
        public virtual SinglePrescription SinglePrescription { get; set; }
        public virtual Prescription Prescription { get; set; }

        public Dispense Clone()
        {
            return new Dispense
            {
                Date = Date,
                Remark = Remark,
                DrugItems = DrugItems.Select(di => di.Clone()).ToList()
            };
        }

        public bool Equals(Dispense other)
        {
            if (other == null)
            {
                return false;
            }
            bool isEqual = false;
            if (DrugItems != null && other.DrugItems != null)
            {
                isEqual = DrugItems.SequenceEqual(other.DrugItems);
            }

            if (Prescription != null && other.Prescription != null)
            {
                isEqual = isEqual && Prescription.Equals(other.Prescription);
            }

            return isEqual && Nullable.Equals(Date, other.Date) && Remark == other.Remark;
        }

        public void Update(Dispense other)
        {
            if (other != null)
            {
                if (Id != other.Id)
                {
                    throw new NotSupportedException("Dispenses with different IDs cannot be merged");
                }
                Date = other.Date;
                Remark = other.Remark;
                DrugItems = new List<DrugItem>(other.DrugItems);
                Prescription = other.Prescription;
                SinglePrescription = other.SinglePrescription;
                StandingPrescription = other.StandingPrescription;
            }
        }
    }
}