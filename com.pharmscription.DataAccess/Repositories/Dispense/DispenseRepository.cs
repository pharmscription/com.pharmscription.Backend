namespace com.pharmscription.DataAccess.Repositories.Dispense
{
    using com.pharmscription.DataAccess.Entities.DispenseEntity;
    using com.pharmscription.DataAccess.Repositories.BaseRepository;
    using com.pharmscription.DataAccess.UnitOfWork;

    public class DispenseRepository : Repository<Dispense>, IDispenseRepository
    {
        public DispenseRepository(IPharmscriptionUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}