namespace com.pharmscription.DataAccess.Entities.DrugStoreEntity
{
    using AddressEntity;
    using BaseEntity;

    public class DrugStore: Entity
    {
        public string Name { get; set; }
        public Address Address { get; set; }
    }
}
