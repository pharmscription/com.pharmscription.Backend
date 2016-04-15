namespace com.pharmscription.DataAccess.Entities.DrugItemEntity
{
    using com.pharmscription.DataAccess.Entities.BaseEntity;
    using com.pharmscription.DataAccess.Entities.DispenseEntity;
    using com.pharmscription.DataAccess.Entities.DrugEntity;
    using com.pharmscription.DataAccess.Entities.PrescriptionEntity;
    using com.pharmscription.DataAccess.SharedInterfaces;
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