namespace com.pharmscription.DataAccess.Repositories.DrugPrice
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using BaseRepository;
    using Entities.DrugPriceEntity;
    using Infrastructure.Exception;
    using UnitOfWork;

    public class DrugPriceRepository :Repository<DrugPrice>, IDrugPriceRepository
    {
        public DrugPriceRepository(IPharmscriptionUnitOfWork unitOfWork) : base(unitOfWork)
        {
            
        }

        public async Task<double> GetPrice(Guid drugId, Guid storeId)
        {
            var drugPrice = await Set.Where(e => e.Drug.Id == drugId && e.DrugStore.Id == storeId).FirstOrDefaultAsync();
            if (drugPrice == null)
            {
                throw new InvalidArgumentException("No Drug Price for such a drug in such a store");
            }
            return drugPrice.Price;
        }
    }
}
