using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using com.pharmscription.DataAccess.Repositories.BaseRepository;
using com.pharmscription.DataAccess.UnitOfWork;

namespace com.pharmscription.DataAccess.Repositories.Patient
{
    using System.Collections;
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

        public Task<IEnumerable<Patient>> GetWithAllNavs(Func<Patient, bool> predicate)
        {
            
            return Task.Factory.StartNew(() => Set.Include(e => e.Prescriptions)
                        .Include(e => e.Address)
                        .Include(e => e.Address.CityCode)
                        .Where(predicate));
        }

        public virtual async Task<ICollection<Prescription>> GetPrescriptions(Guid id)
        {
            var ret =  await Set.Where(e => e.Id == id).Include(e => e.Prescriptions).Select(e => e.Prescriptions.Where(a => a.IsValid)).FirstOrDefaultAsync();
            if (ret != null)
            {
                return ret.ToList();
            }
            return new List<Prescription>();
        }

        public async Task<Patient> GetWithAllNavs(Guid id)
        {
            var patient = 
                await Set.Where(e => e.Id == id)
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

        public virtual async Task<ICollection<Patient>> GetAllWithUnreportedDispenses(DateTime lastRespectedDate)
        {
            var patients =
                await
                    Set.Where(e => e.Prescriptions.SelectMany(a => a.Dispenses).Any(c => !c.Reported) && e.CreatedDate > lastRespectedDate)
                        .Include(e => e.Address)
                        .Include(e => e.Prescriptions)
                        .Include(e => e.Prescriptions.Select(b => b.Dispenses))
                        .Include(e => e.Prescriptions.Select(a => a.Dispenses.Select(c => c.DrugItems)))
                        .Include(
                            e =>
                                e.Prescriptions.Select(a => a.Dispenses.Select(c => c.DrugItems.Select(d => d.Drug))))
                        .ToListAsync();
            return patients;
        }
    }
}
