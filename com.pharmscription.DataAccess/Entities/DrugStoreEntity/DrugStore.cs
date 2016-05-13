namespace com.pharmscription.DataAccess.Entities.DrugStoreEntity
{
    using AddressEntity;
    using BaseEntity;
    using SharedInterfaces;

    public class DrugStore: Entity, ICloneable<DrugStore>
    {
        public string Name { get; set; }
        public Address Address { get; set; }
        public DrugStore Clone()
        {
            return new DrugStore
            {
                Name = Name,
                Address = Address.Clone()
            };
        }
    }
}
