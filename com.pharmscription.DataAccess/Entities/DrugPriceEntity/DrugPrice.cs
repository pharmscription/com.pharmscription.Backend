namespace com.pharmscription.DataAccess.Entities.DrugPriceEntity
{
    using BaseEntity;
    using DrugEntity;
    using DrugStoreEntity;

    public class DrugPrice: Entity
    {
        public virtual Drug Drug { get; set; }
        public virtual DrugStore DrugStore { get; set; }
        public double Price { get; set; }
    }
}
