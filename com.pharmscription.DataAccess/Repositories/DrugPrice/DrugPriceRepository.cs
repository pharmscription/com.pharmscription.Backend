namespace com.pharmscription.DataAccess.Repositories.DrugPrice
{
    using BaseRepository;
    using Entities.DrugPriceEntity;
    using UnitOfWork;

    public class DrugPriceRepository :Repository<DrugPrice>, IDrugPriceRepository
    {
        public DrugPriceRepository(IPharmscriptionUnitOfWork unitOfWork) : base(unitOfWork)
        {
            
        }
    }
}
