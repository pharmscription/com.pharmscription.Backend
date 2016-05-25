

namespace com.pharmscription.DataAccess.Repositories.DrugStore
{
    using BaseRepository;
    using Entities.DrugStoreEntity;
    using UnitOfWork;

    public class DrugStoreRepository : Repository<DrugStore>, IDrugStoreRepository
    {
        public DrugStoreRepository(IPharmscriptionUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
