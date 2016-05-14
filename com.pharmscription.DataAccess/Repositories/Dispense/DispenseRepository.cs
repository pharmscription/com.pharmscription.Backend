using com.pharmscription.DataAccess.Repositories.BaseRepository;
using com.pharmscription.DataAccess.UnitOfWork;

namespace com.pharmscription.DataAccess.Repositories.Dispense
{
    public class DispenseRepository : Repository<Entities.DispenseEntity.Dispense>, IDispenseRepository
    {
        public DispenseRepository(IPharmscriptionUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}