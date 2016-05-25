namespace com.pharmscription.DataAccess.Repositories.DrugItem
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using Entities.DrugItemEntity;
    using BaseRepository;
    using UnitOfWork;

    public class DrugItemRepository : Repository<DrugItem>, IDrugItemRepository
    {
        public DrugItemRepository(IPharmscriptionUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public Task<DrugItem> GetWithDrugAsync(Guid drugItemId)
        {
            return Set.Where(e => e.Id == drugItemId).Include(e => e.Drug).FirstOrDefaultAsync();
        }
    }
}