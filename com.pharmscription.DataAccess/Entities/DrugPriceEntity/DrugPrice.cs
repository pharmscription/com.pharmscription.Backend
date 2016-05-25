namespace com.pharmscription.DataAccess.Entities.DrugPriceEntity
{
    using System;
    using BaseEntity;
    using DrugEntity;
    using DrugStoreEntity;
    using SharedInterfaces;

    public class DrugPrice: Entity, ICloneable<DrugPrice>, IEquatable<DrugPrice>
    {
        public virtual Drug Drug { get; set; }
        public virtual DrugStore DrugStore { get; set; }
        public double Price { get; set; }
        public DrugPrice Clone()
        {
            return new DrugPrice
            {
                Price = Price,
                Drug = Drug.Clone(),
                DrugStore = DrugStore.Clone()
            };
        }

        public bool Equals(DrugPrice other)
        {
            if (other == null)
            {
                return false;
            }
            return Drug.Equals(other.Drug) && DrugStore.Equals(other.DrugStore) && Math.Abs(Price - other.Price) < 0.0001;
        }
    }
}
