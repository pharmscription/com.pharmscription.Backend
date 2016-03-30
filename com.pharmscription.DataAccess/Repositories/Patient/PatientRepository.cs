using System.Linq;
using com.pharmscription.DataAccess.Repositories.BaseRepository;
using com.pharmscription.DataAccess.UnitOfWork;

namespace com.pharmscription.DataAccess.Repositories.Patient
{
    public class PatientRepository : Repository<Entities.PatientEntity.Patient>, IPatientRepository
    {
        public PatientRepository(IPharmscriptionUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public Entities.PatientEntity.Patient GetByAhvNumber(string ahvNumber)
        {
            return GetSet().FirstOrDefault(e => e.AhvNumber == ahvNumber);
        }

        public bool Exists(string ahvNumber)
        {
            return GetSet().Any(e => e.AhvNumber == ahvNumber);
        }
    }
}
