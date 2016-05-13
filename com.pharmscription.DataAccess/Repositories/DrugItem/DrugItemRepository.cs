namespace com.pharmscription.DataAccess.Repositories.DrugItem
{
    using Entities.DrugItemEntity;
    using BaseRepository;
    using UnitOfWork;

    public class DrugItemRepository : Repository<DrugItem>, IDrugItemRepository
    {
        public DrugItemRepository(IPharmscriptionUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}