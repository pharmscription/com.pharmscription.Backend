namespace com.pharmscription.DataAccess.Repositories.DrugItem
{
    using com.pharmscription.DataAccess.Entities.DrugItemEntity;
    using com.pharmscription.DataAccess.Repositories.BaseRepository;
    using com.pharmscription.DataAccess.UnitOfWork;

    public class DrugItemRepository : Repository<DrugItem>, IDrugItemRepository
    {
        public DrugItemRepository(IPharmscriptionUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}