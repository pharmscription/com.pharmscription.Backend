namespace com.pharmscription.DataAccess.Repositories.Doctor
{
    using com.pharmscription.DataAccess.Entities.DoctorEntity;
    using com.pharmscription.DataAccess.Repositories.BaseRepository;
    using com.pharmscription.DataAccess.Repositories.Patient;
    using com.pharmscription.DataAccess.UnitOfWork;

    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(IPharmscriptionUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
