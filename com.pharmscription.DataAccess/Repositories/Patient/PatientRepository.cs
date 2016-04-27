using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using com.pharmscription.DataAccess.Entities.PatientEntity;
using com.pharmscription.DataAccess.Repositories.BaseRepository;
using com.pharmscription.DataAccess.UnitOfWork;

namespace com.pharmscription.DataAccess.Repositories.Patient
{
    using Patient = com.pharmscription.DataAccess.Entities.PatientEntity.Patient;

    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(IPharmscriptionUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public Task<Patient> GetByAhvNumber(string ahvNumber)
        {
            return GetSet().FirstOrDefaultAsync(e => e.AhvNumber == ahvNumber);
        }

        public bool Exists(string ahvNumber)
        {
            return GetSet().Any(e => e.AhvNumber == ahvNumber);
        }

        public virtual Task<Patient> GetWithPrescriptions(Guid id)
        {
            return GetSet().Where(e => e.Id == id).Include(e => e.Prescriptions).FirstOrDefaultAsync();
        }

        public IQueryable<Patient> GetWithAllNavs()
        {
            return GetSet().Include(e => e.Prescriptions);
        }
    }
}
