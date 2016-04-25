namespace com.pharmscription.DataAccess.Repositories.Dispense
{
    using Entities.DispenseEntity;
    using BaseRepository;
    using UnitOfWork;

    public class DispenseRepository : Repository<Dispense>, IDispenseRepository
    {
        public DispenseRepository(IPharmscriptionUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}