namespace com.pharmscription.DataAccess.Repositories.Prescription
{
    using com.pharmscription.DataAccess.Entities.PrescriptionEntity;
    using com.pharmscription.DataAccess.Repositories.BaseRepository;
    using com.pharmscription.DataAccess.SharedInterfaces;
    using com.pharmscription.DataAccess.UnitOfWork;

    public class PrescriptionRepository : Repository<Prescription>, IPrescriptionRepository
    {
        public PrescriptionRepository(IPharmscriptionUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
