using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace com.pharmscription.DataAccess.Repositories.Prescription
{
    using Entities.PrescriptionEntity;
    using BaseRepository;
    using Infrastructure.Exception;
    using UnitOfWork;

    public class PrescriptionRepository : Repository<Prescription>, IPrescriptionRepository
    {
        public PrescriptionRepository(IPharmscriptionUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public virtual Task<List<Prescription>> GetByPatientId(Guid patientId)
        {
            return Set.Where(e => e.Patient.Id == patientId).ToListAsync();
        }

        public IQueryable<Prescription> GetWithAllNavs()
        {
            return Set.Include(e => e.Dispenses).Include(e => e.CounterProposals).Include(e => e.Patient);
        }

        public virtual async Task<Prescription> GetWithAllNavsAsynv(Guid id)
        {
            var prescription = 
                await Set
                    .Include(e => e.PrescriptionHistory)
                    .Include(e => e.CounterProposals)
                    .Include(e => e.Dispenses)
                    .Include(e => e.DrugItems)
                    .Include(e => e.Patient).FirstOrDefaultAsync(e => e.Id == id);
            if (prescription == null)
            {
                throw new NotFoundException("No such Prescription");
            }
            return prescription;
        }
    }
}
