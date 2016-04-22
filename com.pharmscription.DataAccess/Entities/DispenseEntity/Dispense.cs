namespace com.pharmscription.DataAccess.Entities.DispenseEntity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BaseEntity;
    using DrugItemEntity;
    using SharedInterfaces;
    public class Dispense : Entity, ICloneable<Dispense>
    {
        public DateTime Date { get; set; }
        public string Remark { get; set; }
        public List<DrugItem> DrugItems { get; set; } 

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