namespace com.pharmscription.DataAccess.Repositories.Patient
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using com.pharmscription.DataAccess.Entities.PatientEntity;
    using com.pharmscription.DataAccess.Repositories.BaseRepository;
    using com.pharmscription.DataAccess.UnitOfWork;

    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(IPharmscriptionUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public Task<Entities.PatientEntity.Patient> GetByAhvNumber(string ahvNumber)
        {
            return GetSet().FirstOrDefaultAsync(e => e.AhvNumber == ahvNumber);
        }

        public bool Exists(string ahvNumber)
        {
            return GetSet().Any(e => e.AhvNumber == ahvNumber);
        }
    }
}
