using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using com.pharmscription.DataAccess.Repositories.BaseRepository;
using com.pharmscription.DataAccess.UnitOfWork;

namespace com.pharmscription.DataAccess.Repositories.Patient
{
    using System.Collections.Generic;
    using Entities.PrescriptionEntity;
    using Infrastructure.Exception;
    using Patient = Entities.PatientEntity.Patient;

    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(IPharmscriptionUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public Task<Patient> GetByAhvNumber(string ahvNumber)
        {
            return Set.Where(e => e.AhvNumber == ahvNumber).Include(e => e.Address.CityCode).FirstOrDefaultAsync();
        }

        public bool Exists(string ahvNumber)
        {
            return Set.Any(e => e.AhvNumber == ahvNumber);
        }

        public virtual Task<Patient> GetWithPrescriptions(Guid id)
        {
            return Set.Where(e => e.Id == id).Include(e => e.Prescriptions).FirstOrDefaultAsync();
        }

        public IQueryable<Patient> GetWithAllNavs()
        {
            return Set.Include(e => e.Prescriptions).Include(e => e.Address).Include(e => e.Address.CityCode);
        }

        public virtual Task<ICollection<Prescription>> GetPrescriptions(Guid id)
        {
            return Set.Where(e => e.Id == id).Include(e => e.Prescriptions).Select(e => e.Prescriptions).FirstOrDefaultAsync();
        }

        public async Task<Patient> GetWithAllNavs(Guid id)
        {
            var patient = 
                await Set                    .Where(e => e.Id == id)
                    .Include(e => e.Prescriptions)
                    .Include(e => e.Address)
                    .Include(e => e.Address.CityCode)
                    .FirstOrDefaultAsync();
            if (patient == null)
            {
                throw new NotFoundException("No Patient with such an Id exists");
            }
            return patient;
        }
    }
}
