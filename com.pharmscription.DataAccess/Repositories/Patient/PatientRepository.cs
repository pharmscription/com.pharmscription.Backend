using System;
using System.Collections.Generic;

namespace com.pharmscription.DataAccess.Repositories.Patient
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using Entities.PatientEntity;
    using BaseRepository;
    using UnitOfWork;

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
