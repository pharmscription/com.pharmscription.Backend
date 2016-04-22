namespace com.pharmscription.DataAccess.Entities.DrugItemEntity
{
    using BaseEntity;
    using DispenseEntity;
    using DrugEntity;
    using PrescriptionEntity;
    using SharedInterfaces;
    public class DrugItem : Entity, ICloneable<DrugItem>
    {
        public Drug Drug { get; set; }
        public Dispense Dispense { get; set; }
        public Prescription Prescription { get; set; }
        public string DosageDescription { get; set; }

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