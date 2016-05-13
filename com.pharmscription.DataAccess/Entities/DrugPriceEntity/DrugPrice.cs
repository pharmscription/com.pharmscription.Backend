namespace com.pharmscription.DataAccess.Entities.DrugPriceEntity
{
    using BaseEntity;
    using DrugEntity;
    using DrugStoreEntity;
    using SharedInterfaces;

    public class DrugPrice: Entity, ICloneable<DrugPrice>
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
    }
}
