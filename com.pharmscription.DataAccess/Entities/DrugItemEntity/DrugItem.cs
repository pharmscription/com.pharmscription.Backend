namespace com.pharmscription.DataAccess.Entities.DrugItemEntity
{
    using BaseEntity;
    using DispenseEntity;
    using DrugEntity;
    using PrescriptionEntity;
    using SharedInterfaces;
    public class DrugItem : Entity, ICloneable<DrugItem>
    {
        public virtual Drug Drug { get; set; }
        public virtual Dispense Dispense { get; set; }
        public virtual Prescription Prescription { get; set; }
        public virtual StandingPrescription StandingPrescription { get; set; }
        public virtual SinglePrescription SinglePrescription { get; set; }
        public string DosageDescription { get; set; }
        public int Quantity { get; set; }
        public DrugItem Clone()
        {
            return new DrugItem
                       {
                           Drug = Drug,
                           Dispense = Dispense,
                           Prescription = Prescription,
                           DosageDescription = DosageDescription,
                       };
        }
    }
}